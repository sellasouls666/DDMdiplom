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
                // Определяем максимальное количество слотов памяти (из материнской платы или по умолчанию 4)
                int maxModules = 4;
                var motherboardComponent = build.FirstOrDefault(c => c.Type == "Материнская плата");
                if (motherboardComponent != null)
                {
                    var motherboard = await _context.Motherboards.FindAsync(motherboardComponent.Id);
                    if (motherboard != null && motherboard.MemorySlots.HasValue)
                        maxModules = motherboard.MemorySlots.Value;
                }

                // Считаем текущее количество модулей
                int currentModules = build
                    .Where(c => c.Type == "Оперативная память")
                    .Sum(c => c.ModuleCount);
                int newModules = component.ModuleCount;
                int totalModules = currentModules + newModules;

                if (totalModules > maxModules)
                {
                    // Если лимит превышен – удаляем самый старый комплект памяти (первый в списке)
                    var oldestRam = build.FirstOrDefault(c => c.Type == "Оперативная память");
                    if (oldestRam != null)
                        build.Remove(oldestRam);
                    // Добавляем новый комплект
                    build.Add(component);
                }
                else
                {
                    build.Add(component);
                }
            }
            else
            {
                // Для остальных компонентов: заменяем старый тем же типом
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
            var item = build.FirstOrDefault(c => c.Id == request.Id);
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