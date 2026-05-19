using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class StorageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StorageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Storage (страница выбора типа накопителя)
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Storage/HardDrives
        public async Task<IActionResult> HardDrives()
        {
            var hardDrives = await _context.Storages
                .Where(s => s.DeviceType == "HDD")
                .ToListAsync();
            return View(hardDrives);
        }

        // GET: /Storage/Ssds (заглушка)
        public IActionResult Ssds()
        {
            // Позже реализуем
            return Content("Страница со списком SSD будет позже");
        }

        // GET: /Storage/GetHardDrive/{id}
        [HttpGet]
        public async Task<IActionResult> GetHardDrive(int id)
        {
            var drive = await _context.Storages.FindAsync(id);
            if (drive == null) return NotFound();
            return Ok(drive);
        }
    }
}