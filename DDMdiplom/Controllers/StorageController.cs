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

        // GET: /Storage/Ssds
        public async Task<IActionResult> Ssds()
        {
            var ssds = await _context.Storages
                .Where(s => s.DeviceType != null &&
                            (s.DeviceType.Contains("SSD") || s.DeviceType.Contains("NVMe")))
                .ToListAsync();
            return View(ssds);
        }

        // GET: /Storage/GetStorage/{id} (универсальный метод для деталей)
        [HttpGet]
        public async Task<IActionResult> GetStorage(int id)
        {
            var storage = await _context.Storages.FindAsync(id);
            if (storage == null) return NotFound();
            return Ok(storage);
        }
    }
}