using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class MonitorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonitorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Monitors – выбор типа монитора
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Monitors/GamingMonitors – игровые мониторы
        public async Task<IActionResult> GamingMonitors()
        {
            var monitors = await _context.Monitors
                .Where(m => m.MonitorType != null &&
                            (m.MonitorType.Contains("Gaming") || m.MonitorType.Contains("Игровой")))
                .ToListAsync();
            return View(monitors);
        }

        // GET: /Monitors/LcdMonitors – LCD/LED мониторы (заглушка)
        public IActionResult LcdMonitors()
        {
            return View();
        }

        // GET: /Monitors/GetMonitor/{id} – JSON для модального окна
        [HttpGet]
        public async Task<IActionResult> GetMonitor(int id)
        {
            var monitor = await _context.Monitors.FindAsync(id);
            if (monitor == null) return NotFound();
            return Json(monitor);
        }
    }
}