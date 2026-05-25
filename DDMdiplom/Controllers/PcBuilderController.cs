using DDMdiplom.Data;
using DDMdiplom.Models;
using DDMdiplom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DDMdiplom.Controllers
{
    public class PcBuilderController : Controller
    {
        private const string BuildSessionKey = "CurrentBuild";
        private readonly ApplicationDbContext _context;

        public PcBuilderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("PcBuilder")]
        public IActionResult Index()
        {
            return View();
        }

        private List<BuildComponent> GetBuildFromSession()
        {
            var json = HttpContext.Session.GetString(BuildSessionKey);
            return string.IsNullOrEmpty(json)
                ? new List<BuildComponent>()
                : JsonSerializer.Deserialize<List<BuildComponent>>(json);
        }

        private void SaveBuildToSession(List<BuildComponent> build)
        {
            var json = JsonSerializer.Serialize(build);
            HttpContext.Session.SetString(BuildSessionKey, json);
        }

        [HttpGet]
        public IActionResult GetBuild()
        {
            var build = GetBuildFromSession();
            return Json(new { success = true, build });
        }

        [HttpPost]
        public async Task<IActionResult> AddComponent([FromBody] BuildComponent component)
        {
            var build = GetBuildFromSession();

            if (component.Type == "Материнская плата")
            {
                var motherboard = await _context.Motherboards.FindAsync(component.Id);
                if (motherboard != null)
                {
                    // Количество SATA-портов
                    if (!string.IsNullOrEmpty(motherboard.SataPorts))
                    {
                        var matchSata = Regex.Match(motherboard.SataPorts, @"(\d+)\s*x\s*SATA");
                        if (matchSata.Success)
                            component.SataPorts = int.Parse(matchSata.Groups[1].Value);
                    }
                    // Количество слотов M.2
                    if (!string.IsNullOrEmpty(motherboard.SataPorts))
                    {
                        var matchM2 = Regex.Match(motherboard.SataPorts, @"(\d+)\s*x\s*M\.2", RegexOptions.IgnoreCase);
                        if (matchM2.Success)
                            component.M2Slots = int.Parse(matchM2.Groups[1].Value);
                    }
                    // Количество слотов памяти
                    component.MemorySlots = motherboard.MemorySlots;
                    component.MbMemoryStandard = motherboard.MemoryStandard;
                    component.CpuSocketType = motherboard.CpuSocketType;
                    component.MbFormFactor = motherboard.FormFactor;
                }
            }

            // Специальная обработка для оперативной памяти
            if (component.Type == "Оперативная память")
            {
                var memory = await _context.Memories.FindAsync(component.Id);
                if (memory != null)
                {
                    component.Speed = memory.Speed;  // сохраняем строку типа "DDR5 6400"
                }
                int maxModules = 4;
                var motherboardComponent = build.FirstOrDefault(c => c.Type == "Материнская плата");
                if (motherboardComponent != null && motherboardComponent.MemorySlots.HasValue)
                {
                    maxModules = motherboardComponent.MemorySlots.Value;
                }
                else if (motherboardComponent != null)
                {
                    // fallback – запрос в БД (если поле не сохранено)
                    var motherboard = await _context.Motherboards.FindAsync(motherboardComponent.Id);
                    if (motherboard != null && motherboard.MemorySlots.HasValue)
                        maxModules = motherboard.MemorySlots.Value;
                }

                int currentModules = build
                    .Where(c => c.Type == "Оперативная память")
                    .Sum(c => c.ModuleCount);
                int totalModules = currentModules + component.ModuleCount;

                if (totalModules > maxModules)
                {
                    var oldestRam = build.FirstOrDefault(c => c.Type == "Оперативная память");
                    if (oldestRam != null) build.Remove(oldestRam);
                    build.Add(component);
                }
                else
                {
                    build.Add(component);
                }
            }
            // Специальная обработка для накопителей
            else if (component.Type == "Накопитель")
            {
                // Проверяем, выбрана ли материнская плата
                var motherboardComponent = build.FirstOrDefault(c => c.Type == "Материнская плата");

                if (motherboardComponent == null)
                {
                    // Без материнки: общий лимит накопителей = 2 (любые)
                    int totalDrives = build.Count(c => c.Type == "Накопитель");
                    if (totalDrives >= 2)
                    {
                        var oldest = build.FirstOrDefault(c => c.Type == "Накопитель");
                        if (oldest != null) build.Remove(oldest);
                    }
                    build.Add(component);
                }
                else
                {
                    // С материнской платой: разделяем SATA и M.2
                    int maxSata = motherboardComponent.SataPorts ?? 2; // значение по умолчанию
                    int maxM2 = motherboardComponent.M2Slots ?? 2;

                    // Подсчитываем текущее количество накопителей каждого интерфейса
                    int currentSata = build.Count(c => c.Type == "Накопитель" &&
                        (c.StorageInterface == "SATA" || string.IsNullOrEmpty(c.StorageInterface))); // HDD без указания интерфейса считаем SATA
                    int currentM2 = build.Count(c => c.Type == "Накопитель" && c.StorageInterface == "M.2");

                    // Определяем интерфейс нового компонента (если не задан – считаем SATA для HDD)
                    string newInterface = component.StorageInterface ?? "SATA";

                    if (newInterface == "SATA")
                    {
                        if (currentSata >= maxSata)
                        {
                            var oldestSata = build.FirstOrDefault(c => c.Type == "Накопитель" &&
                                (c.StorageInterface == "SATA" || string.IsNullOrEmpty(c.StorageInterface)));
                            if (oldestSata != null) build.Remove(oldestSata);
                        }
                        build.Add(component);
                    }
                    else if (newInterface == "M.2")
                    {
                        if (currentM2 >= maxM2)
                        {
                            var oldestM2 = build.FirstOrDefault(c => c.Type == "Накопитель" && c.StorageInterface == "M.2");
                            if (oldestM2 != null) build.Remove(oldestM2);
                        }
                        build.Add(component);
                    }
                }
            }
            // Видеокарта
            else if (component.Type == "Видеокарта")
            {
                var gpu = await _context.GraphicsCards.FindAsync(component.Id);
                if (gpu != null) component.GpuLength = gpu.MaxGpuLength;
                var existing = build.FirstOrDefault(c => c.Type == component.Type);
                if (existing != null) build.Remove(existing);
                build.Add(component);
            }
            // Блок питания
            else if (component.Type == "Блок питания")
            {
                var psu = await _context.PowerSupplies.FindAsync(component.Id);
                if (psu != null) component.PsuLength = psu.MaxPsuLength;
                var existing = build.FirstOrDefault(c => c.Type == component.Type);
                if (existing != null) build.Remove(existing);
                build.Add(component);
            }
            else if (component.Type == "Монитор")
            {
                const int maxMonitors = 2;
                int currentMonitors = build.Count(c => c.Type == "Монитор");
                if (currentMonitors >= maxMonitors)
                {
                    var oldestMonitor = build.FirstOrDefault(c => c.Type == "Монитор");
                    if (oldestMonitor != null) build.Remove(oldestMonitor);
                }
                build.Add(component);
            }
            else // все остальные компоненты (в том числе кулеры)
            {
                if (component.Type == "Процессор")
                {
                    var processor = await _context.Processors.FindAsync(component.Id);
                    if (processor != null)
                    {
                        component.ProcessorSocket = processor.CpuSocketType;
                        component.ProcessorMemoryTypes = processor.MemoryTypes;
                    }
                }

                if (component.Type == "Корпус")
                {
                    var pcCase = await _context.Cases.FindAsync(component.Id);
                    if (pcCase != null)
                    {
                        component.MaxGpuLength = pcCase.MaxGpuLength;
                        component.CaseMotherboardFormFactors = pcCase.MotherboardCompatibility;
                        component.MaxPsuLength = pcCase.MaxPsuLength;
                    }
                }

                // если это система охлаждения – сохраняем совместимость
                if (component.Type == "Система охлаждения")
                {
                    // пробуем найти воздушный кулер
                    var airCooler = await _context.CpuCoolers.FindAsync(component.Id);
                    if (airCooler != null)
                    {
                        component.CpuSocketCompatibility = airCooler.CpuSocketCompatibility;
                    }
                    else
                    {
                        // ищем водяное охлаждение
                        var waterCooler = await _context.WaterCoolers.FindAsync(component.Id);
                        if (waterCooler != null)
                        {
                            component.BlockCompatibility = waterCooler.BlockCompatibility;
                        }
                    }
                }

                var existing = build.FirstOrDefault(c => c.Type == component.Type);
                if (existing != null)
                    build.Remove(existing);

                build.Add(component);
            }

            SaveBuildToSession(build);
            return Json(new { success = true, build });
        }

        [HttpPost]
        public IActionResult RemoveComponent([FromBody] RemoveComponentRequest request)
        {
            var build = GetBuildFromSession();
            var item = build.FirstOrDefault(c => c.InstanceId == request.InstanceId);
            if (item != null) build.Remove(item);
            SaveBuildToSession(build);
            return Json(new { success = true, build });
        }

        [HttpPost]
        public IActionResult ClearBuild()
        {
            HttpContext.Session.Remove(BuildSessionKey);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> SaveBuild([FromBody] SaveBuildRequest request)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false, error = "Необходимо войти в систему" });

            if (request.Components == null || request.Components.Count == 0)
                return Json(new { success = false, error = "Сборка пуста" });

            int? buildId = request.BuildId;

            // Если buildId не передан в запросе, пробуем взять из сессии (для редактирования)
            if (!buildId.HasValue)
            {
                var editingBuildId = HttpContext.Session.GetInt32("EditingBuildId");
                if (editingBuildId.HasValue)
                    buildId = editingBuildId;
            }

            Build build;
            if (buildId.HasValue)
            {
                build = await _context.Builds.Include(b => b.Items)
                    .FirstOrDefaultAsync(b => b.Id == buildId && b.UserId == userId);
                if (build == null)
                    return Json(new { success = false, error = "Сборка не найдена" });
                // Удаляем старые элементы
                _context.BuildItems.RemoveRange(build.Items);
                build.Items.Clear();
                build.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                build = new Build
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Builds.Add(build);
            }

            build.Name = request.Name ?? "Моя конфигурация";

            foreach (var comp in request.Components)
            {
                var item = new BuildItem
                {
                    ComponentType = comp.Type,
                    ComponentId = comp.Id,
                    ModuleCount = comp.ModuleCount,
                    StorageInterface = comp.StorageInterface
                };
                build.Items.Add(item);
            }

            await _context.SaveChangesAsync();

            // Очищаем EditingBuildId в сессии после сохранения
            HttpContext.Session.Remove("EditingBuildId");

            return Json(new { success = true, buildId = build.Id });
        }

        [Route("PcBuilder/ClearAndGoToProcessors")]
        public IActionResult ClearAndGoToProcessors()
        {
            HttpContext.Session.Remove(BuildSessionKey);
            return RedirectToAction("Index", "Processors");
        }

        [Route("PcBuilder/EditBuild/{id}")]
        public async Task<IActionResult> EditBuild(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var build = await _context.Builds
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (build == null)
                return NotFound();

            // Очищаем текущую сессию
            HttpContext.Session.Remove(BuildSessionKey);

            var components = new List<BuildComponent>();

            foreach (var item in build.Items)
            {
                var comp = new BuildComponent
                {
                    Id = item.ComponentId,
                    Type = item.ComponentType,
                    ModuleCount = item.ModuleCount,
                    StorageInterface = item.StorageInterface,
                    InstanceId = Guid.NewGuid()
                };

                switch (item.ComponentType)
                {
                    case "Процессор":
                        var cpu = await _context.Processors.FindAsync(item.ComponentId);
                        if (cpu != null)
                        {
                            comp.Name = $"{cpu.Brand} {cpu.Name}";
                            comp.Price = cpu.Price;
                            comp.ImageUrl = cpu.ImageUrl;
                            comp.Tdp = cpu.ThermalDesignPower;
                            comp.ProcessorSocket = cpu.CpuSocketType;
                            comp.ProcessorMemoryTypes = cpu.MemoryTypes;
                        }
                        break;

                    case "Материнская плата":
                        var mb = await _context.Motherboards.FindAsync(item.ComponentId);
                        if (mb != null)
                        {
                            comp.Name = $"{mb.Brand} {mb.Model}";
                            comp.Price = mb.Price;
                            comp.ImageUrl = mb.ImageUrl;
                            comp.CpuSocketType = mb.CpuSocketType;
                            comp.MemorySlots = mb.MemorySlots;
                            comp.MbMemoryStandard = mb.MemoryStandard;
                            comp.MbFormFactor = mb.FormFactor;
                            // SATA порты
                            if (!string.IsNullOrEmpty(mb.SataPorts))
                            {
                                var matchSata = Regex.Match(mb.SataPorts, @"(\d+)\s*x\s*SATA");
                                if (matchSata.Success) comp.SataPorts = int.Parse(matchSata.Groups[1].Value);
                            }
                            // M.2 слоты
                            if (!string.IsNullOrEmpty(mb.SataPorts))
                            {
                                var matchM2 = Regex.Match(mb.SataPorts, @"(\d+)\s*x\s*M\.2", RegexOptions.IgnoreCase);
                                if (matchM2.Success) comp.M2Slots = int.Parse(matchM2.Groups[1].Value);
                            }
                        }
                        break;

                    case "Видеокарта":
                        var gpu = await _context.GraphicsCards.FindAsync(item.ComponentId);
                        if (gpu != null)
                        {
                            comp.Name = $"{gpu.Brand} {gpu.Model}";
                            comp.Price = gpu.Price;
                            comp.ImageUrl = gpu.ImageUrl;
                            comp.Tdp = gpu.ThermalDesignPower;
                            comp.GpuLength = gpu.MaxGpuLength;
                        }
                        break;

                    case "Оперативная память":
                        var ram = await _context.Memories.FindAsync(item.ComponentId);
                        if (ram != null)
                        {
                            comp.Name = $"{ram.Brand} {ram.Model}";
                            comp.Price = ram.Price;
                            comp.ImageUrl = ram.ImageUrl;
                            comp.Speed = ram.Speed;
                            comp.ModuleCount = item.ModuleCount;
                        }
                        break;

                    case "Накопитель":
                        var storage = await _context.Storages.FindAsync(item.ComponentId);
                        if (storage != null)
                        {
                            comp.Name = $"{storage.Brand} {storage.Model}";
                            comp.Price = storage.Price;
                            comp.ImageUrl = storage.ImageUrl;
                            comp.StorageInterface = item.StorageInterface;
                        }
                        break;

                    case "Блок питания":
                        var psu = await _context.PowerSupplies.FindAsync(item.ComponentId);
                        if (psu != null)
                        {
                            comp.Name = $"{psu.Brand} {psu.Model}";
                            comp.Price = psu.Price;
                            comp.ImageUrl = psu.ImageUrl;
                            comp.PsuLength = psu.MaxPsuLength;
                            comp.PsuMaximumPower = psu.MaximumPower;
                        }
                        break;

                    case "Корпус":
                        var pcCase = await _context.Cases.FindAsync(item.ComponentId);
                        if (pcCase != null)
                        {
                            comp.Name = $"{pcCase.Brand} {pcCase.Model}";
                            comp.Price = pcCase.Price;
                            comp.ImageUrl = pcCase.ImageUrl;
                            comp.MaxGpuLength = pcCase.MaxGpuLength;
                            comp.CaseMotherboardFormFactors = pcCase.MotherboardCompatibility;
                            comp.MaxPsuLength = pcCase.MaxPsuLength;
                        }
                        break;

                    case "Система охлаждения":
                        var air = await _context.CpuCoolers.FindAsync(item.ComponentId);
                        if (air != null)
                        {
                            comp.Name = $"{air.Brand} {air.Model}";
                            comp.Price = air.Price;
                            comp.ImageUrl = air.ImageUrl;
                            comp.CpuSocketCompatibility = air.CpuSocketCompatibility;
                        }
                        else
                        {
                            var water = await _context.WaterCoolers.FindAsync(item.ComponentId);
                            if (water != null)
                            {
                                comp.Name = $"{water.Brand} {water.Model}";
                                comp.Price = water.Price;
                                comp.ImageUrl = water.ImageUrl;
                                comp.BlockCompatibility = water.BlockCompatibility;
                            }
                        }
                        break;

                    case "Операционная система":
                        var os = await _context.OperatingSystems.FindAsync(item.ComponentId);
                        if (os != null)
                        {
                            comp.Name = $"{os.Brand} {os.Name}";
                            comp.Price = os.Price;
                            comp.ImageUrl = os.ImageUrl;
                        }
                        break;

                    case "Монитор":
                        var monitor = await _context.Monitors.FindAsync(item.ComponentId);
                        if (monitor != null)
                        {
                            comp.Name = $"{monitor.Brand} {monitor.Model}";
                            comp.Price = monitor.Price;
                            comp.ImageUrl = monitor.ImageUrl;
                        }
                        break;

                    case "Источник бесперебойного питания":
                        var ups = await _context.UpsDevices.FindAsync(item.ComponentId);
                        if (ups != null)
                        {
                            comp.Name = $"{ups.Brand} {ups.Model}";
                            comp.Price = ups.Price;
                            comp.ImageUrl = ups.ImageUrl;
                        }
                        break;

                    case "Клавиатура":
                        var kb = await _context.Keyboards.FindAsync(item.ComponentId);
                        if (kb != null)
                        {
                            comp.Name = $"{kb.Brand} {kb.Name ?? kb.Model}";
                            comp.Price = kb.Price;
                            comp.ImageUrl = kb.ImageUrl;
                        }
                        break;

                    case "Мышь":
                        var mouse = await _context.Mice.FindAsync(item.ComponentId);
                        if (mouse != null)
                        {
                            comp.Name = $"{mouse.Brand} {mouse.Name}";
                            comp.Price = mouse.Price;
                            comp.ImageUrl = mouse.ImageUrl;
                        }
                        break;
                }

                components.Add(comp);
            }

            SaveBuildToSession(components);
            HttpContext.Session.SetInt32("EditingBuildId", id);

            return Redirect($"/Processors?editingBuildId={id}");
        }
    }
}