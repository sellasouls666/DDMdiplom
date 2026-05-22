using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class MiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Mice – выбор типа мыши
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Mice/GamingMice – игровые мыши
        public async Task<IActionResult> GamingMice()
        {
            var mice = await _context.Mice
                .Where(m => m.Type != null &&
                            (m.Type.Contains("Gaming") || m.Type.Contains("Игровая")))
                .ToListAsync();
            return View(mice);
        }

        // GET: /Mice/Mice – обычные мыши
        public async Task<IActionResult> Mice()
        {
            var mice = await _context.Mice
                .Where(m => m.Type != null &&
                            !m.Type.Contains("Gaming") && !m.Type.Contains("Игровая"))
                .ToListAsync();
            return View(mice);
        }

        // GET: /Mice/GetMouse/{id} – JSON для модального окна
        [HttpGet]
        public async Task<IActionResult> GetMouse(int id)
        {
            var mouse = await _context.Mice.FindAsync(id);
            if (mouse == null) return NotFound();
            return Json(mouse);
        }
    }
}