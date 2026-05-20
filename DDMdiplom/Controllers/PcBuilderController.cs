using DDMdiplom.Data;
using DDMdiplom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

            // Специальная обработка для оперативной памяти
            if (component.Type == "Оперативная память")
            {
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
                int maxDrives = 2; // значение по умолчанию
                var motherboardComponent = build.FirstOrDefault(c => c.Type == "Материнская плата");
                if (motherboardComponent != null && motherboardComponent.SataPorts.HasValue)
                {
                    maxDrives = motherboardComponent.SataPorts.Value;
                }
                else if (motherboardComponent != null)
                {
                    var motherboard = await _context.Motherboards.FindAsync(motherboardComponent.Id);
                    if (motherboard != null && !string.IsNullOrEmpty(motherboard.SataPorts))
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(motherboard.SataPorts, @"(\d+)\s*x\s*SATA");
                        if (match.Success)
                            maxDrives = int.Parse(match.Groups[1].Value);
                    }
                }

                int currentDrives = build.Count(c => c.Type == "Накопитель");
                if (currentDrives >= maxDrives)
                {
                    var oldestDrive = build.FirstOrDefault(c => c.Type == "Накопитель");
                    if (oldestDrive != null) build.Remove(oldestDrive);
                    build.Add(component);
                }
                else
                {
                    build.Add(component);
                }
            }
            else
            {
                // Обработка всех остальных компонентов (включая материнскую плату)
                var existing = build.FirstOrDefault(c => c.Type == component.Type);
                if (existing != null) build.Remove(existing);

                // Если добавляется материнская плата – сохраняем дополнительные параметры
                if (component.Type == "Материнская плата")
                {
                    var motherboard = await _context.Motherboards.FindAsync(component.Id);
                    if (motherboard != null)
                    {
                        // Количество SATA-портов
                        if (!string.IsNullOrEmpty(motherboard.SataPorts))
                        {
                            var matchSata = System.Text.RegularExpressions.Regex.Match(motherboard.SataPorts, @"(\d+)\s*x\s*SATA");
                            if (matchSata.Success)
                                component.SataPorts = int.Parse(matchSata.Groups[1].Value);
                        }
                        // Количество слотов памяти
                        component.MemorySlots = motherboard.MemorySlots;
                    }
                }

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
    }
}