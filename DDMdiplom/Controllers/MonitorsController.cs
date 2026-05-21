using Microsoft.AspNetCore.Mvc;

namespace DDMdiplom.Controllers
{
    public class MonitorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GamingMonitors()
        {
            return View();
        }

        public IActionResult LcdMonitors()
        {
            return View();
        }
    }
}