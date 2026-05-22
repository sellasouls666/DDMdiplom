using Microsoft.AspNetCore.Mvc;

namespace DDMdiplom.Controllers
{
    public class MiceController : Controller
    {
        // GET: /Mice – выбор типа мыши
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Mice/GamingMice – игровые мыши
        public IActionResult GamingMice()
        {
            // Временно без БД – позже можно загрузить из контекста
            return View();
        }

        // GET: /Mice/Mice – обычные мыши
        public IActionResult Mice()
        {
            return View();
        }
    }
}