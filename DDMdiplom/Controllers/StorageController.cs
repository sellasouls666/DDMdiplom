using Microsoft.AspNetCore.Mvc;

namespace DDMdiplom.Controllers
{
    public class StorageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HardDrives()
        {
            // Пока просто возвращаем пустую страницу, потом заменим на список HDD
            return View();
        }

        public IActionResult Ssds()
        {
            // Пока просто возвращаем пустую страницу, потом заменим на список SSD
            return View();
        }
    }
}