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
        public async Task<IActionResult> GetBuild()
        {
            var build = GetBuildFromSession();
            string buildName = "Моя конфигурация";

            var editingBuildId = HttpContext.Session.GetInt32("EditingBuildId");
            if (editingBuildId.HasValue)
            {
                var currentBuildDb = await _context.Builds.FindAsync(editingBuildId.Value);
                if (currentBuildDb != null && !string.IsNullOrEmpty(currentBuildDb.Name))
                {
                    buildName = currentBuildDb.Name;
                }
            }
            else
            {
                var sessionBuildName = HttpContext.Session.GetString("BuildName");
                if (!string.IsNullOrEmpty(sessionBuildName))
                {
                    buildName = sessionBuildName;
                }
            }

            return Json(new { success = true, build, buildName });
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
                    component.SataPorts = ParseSataPortCount(motherboard.SataPorts);
                    component.M2Slots = ParseM2SlotCount(motherboard.M2Slots);
                    component.MemorySlots = motherboard.MemorySlots;
                    component.MbMemoryStandard = motherboard.MemoryStandard;
                    component.CpuSocketType = motherboard.CpuSocketType;
                    component.MbFormFactor = motherboard.FormFactor;
                }
            }

            if (component.Type == "Оперативная память")
            {
                var memory = await _context.Memories.FindAsync(component.Id);
                if (memory != null)
                {
                    component.Speed = memory.Speed;
                }
                int maxModules = 4;
                var motherboardComponent = build.FirstOrDefault(c => c.Type == "Материнская плата");
                if (motherboardComponent != null && motherboardComponent.MemorySlots.HasValue)
                {
                    maxModules = motherboardComponent.MemorySlots.Value;
                }
                else if (motherboardComponent != null)
                {
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
            else if (component.Type == "Накопитель")
            {
                var motherboardComponent = build.FirstOrDefault(c => c.Type == "Материнская плата");

                if (motherboardComponent == null)
                {
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
                    int maxSata = motherboardComponent.SataPorts ?? 2;
                    int maxM2 = motherboardComponent.M2Slots ?? 2;

                    int currentSata = build.Count(c => c.Type == "Накопитель" &&
                        (c.StorageInterface == "SATA" || string.IsNullOrEmpty(c.StorageInterface)));
                    int currentM2 = build.Count(c => c.Type == "Накопитель" && c.StorageInterface == "M.2");

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
            else if (component.Type == "Видеокарта")
            {
                var gpu = await _context.GraphicsCards.FindAsync(component.Id);
                if (gpu != null) component.GpuLength = gpu.MaxGpuLength;
                var existing = build.FirstOrDefault(c => c.Type == component.Type);
                if (existing != null) build.Remove(existing);
                build.Add(component);
            }
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
            else
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

                if (component.Type == "Система охлаждения")
                {
                    var airCooler = await _context.CpuCoolers.FindAsync(component.Id);
                    if (airCooler != null)
                    {
                        component.CpuSocketCompatibility = airCooler.CpuSocketCompatibility;
                    }
                    else
                    {
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
                            comp.SataPorts = ParseSataPortCount(mb.SataPorts);
                            comp.M2Slots = ParseM2SlotCount(mb.M2Slots);
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

        [HttpGet]
        public async Task<IActionResult> GetBuildById(int buildId)
        {
            var build = await _context.Builds
                .Include(b => b.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == buildId);

            if (build == null)
            {
                return NotFound(new { success = false, error = "Конфигурация не найдена" });
            }

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
                            // Правильно парсим SATA и M.2
                            comp.SataPorts = ParseSataPortCount(mb.SataPorts);
                            comp.M2Slots = ParseM2SlotCount(mb.M2Slots);
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

                if (!string.IsNullOrEmpty(comp.Name))
                {
                    components.Add(comp);
                }
            }

            return Json(new { success = true, components });
        }

        public class SetBuildNameRequest
        {
            public string Name { get; set; } = string.Empty;
        }

        [HttpPost]
        public IActionResult SetBuildName([FromBody] SetBuildNameRequest request)
        {
            HttpContext.Session.SetString("BuildName", request.Name);
            return Ok();
        }

        private int ParseSataPortCount(string sataPortsString)
        {
            if (string.IsNullOrEmpty(sataPortsString)) return 0;
            var match = Regex.Match(sataPortsString, @"(\d+)\s*x\s*SATA", RegexOptions.IgnoreCase);
            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
        }

        private int ParseM2SlotCount(string m2SlotsString)
        {
            if (string.IsNullOrEmpty(m2SlotsString)) return 0;
            // Ищем "цифра x M.2"
            var match = Regex.Match(m2SlotsString, @"(\d+)\s*x\s*M\.2", RegexOptions.IgnoreCase);
            if (match.Success) return int.Parse(match.Groups[1].Value);
            // Если нет, считаем количество упоминаний "M.2"
            return Regex.Matches(m2SlotsString, @"M\.2", RegexOptions.IgnoreCase).Count;
        }
    }
}
