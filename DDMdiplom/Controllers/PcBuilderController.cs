using DDMdiplom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DDMdiplom.Controllers
{
    public class PcBuilderController : Controller
    {
        private const string BuildSessionKey = "CurrentBuild";

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
        public IActionResult AddComponent([FromBody] BuildComponent component)
        {
            var build = GetBuildFromSession();
            // Удаляем старый компонент того же типа (например, "Процессор")
            var existing = build.FirstOrDefault(c => c.Type == component.Type);
            if (existing != null)
                build.Remove(existing);
            build.Add(component);
            SaveBuildToSession(build);
            return Json(new { success = true, build });
        }

        [HttpPost]
        public IActionResult RemoveComponent([FromBody] RemoveComponentRequest request)
        {
            var build = GetBuildFromSession();
            var item = build.FirstOrDefault(c => c.Id == request.Id);
            if (item != null)
                build.Remove(item);
            SaveBuildToSession(build);
            return Json(new { success = true, build });
        }
    }
}