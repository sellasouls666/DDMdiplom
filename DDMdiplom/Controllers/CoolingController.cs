using Microsoft.AspNetCore.Mvc;

namespace DDMdiplom.Controllers
{
    public class CoolingController : Controller
    {
        // GET: /Cooling – выбор типа охлаждения
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Cooling/AirCoolers – воздушные кулеры (Fan & Heatsink)
        public IActionResult AirCoolers()
        {
            // В будущем замените на получение данных из БД
            return View();
        }

        // GET: /Cooling/LiquidCoolers – водяные системы (Water Cooler Kit)
        public IActionResult LiquidCoolers()
        {
            // В будущем замените на получение данных из БД
            return View();
        }
    }
}