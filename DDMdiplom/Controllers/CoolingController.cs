using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class CoolingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoolingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Cooling – выбор типа охлаждения
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Cooling/AirCoolers – воздушные кулеры
        public async Task<IActionResult> AirCoolers()
        {
            var coolers = await _context.CpuCoolers.ToListAsync();
            return View(coolers);
        }

        // GET: /Cooling/GetCpuCooler/{id} – JSON для модального окна
        [HttpGet]
        public async Task<IActionResult> GetCpuCooler(int id)
        {
            var cooler = await _context.CpuCoolers.FindAsync(id);
            if (cooler == null) return NotFound();
            return Json(cooler);
        }

        // ======== ЖИДКОСТНОЕ ОХЛАЖДЕНИЕ ========
        // GET: /Cooling/LiquidCoolers
        public async Task<IActionResult> LiquidCoolers()
        {
            var waterCoolers = await _context.WaterCoolers.ToListAsync();
            return View(waterCoolers);
        }

        // GET: /Cooling/GetWaterCooler/{id}
        [HttpGet]
        public async Task<IActionResult> GetWaterCooler(int id)
        {
            var waterCooler = await _context.WaterCoolers.FindAsync(id);
            if (waterCooler == null) return NotFound();
            return Json(waterCooler);
        }
    }
}