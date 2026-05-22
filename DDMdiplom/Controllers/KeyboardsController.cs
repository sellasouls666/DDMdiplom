using Microsoft.AspNetCore.Mvc;

namespace DDMdiplom.Controllers
{
    public class KeyboardsController : Controller
    {
        // GET: /Keyboards – выбор типа клавиатуры
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Keyboards/GamingKeyboards – игровые клавиатуры
        public IActionResult GamingKeyboards()
        {
            // Временно без БД
            return View();
        }

        // GET: /Keyboards/Keyboards – обычные клавиатуры
        public IActionResult Keyboards()
        {
            return View();
        }
    }
}