using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class KeyboardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KeyboardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Keyboards – выбор типа клавиатуры
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Keyboards/GamingKeyboards – игровые клавиатуры
        public async Task<IActionResult> GamingKeyboards()
        {
            var keyboards = await _context.Keyboards
                .Where(k => k.Type != null &&
                            (k.Type.Contains("Gaming") || k.Type.Contains("Игровая")))
                .ToListAsync();
            return View(keyboards);
        }

        // GET: /Keyboards/Keyboards – обычные клавиатуры
        public async Task<IActionResult> Keyboards()
        {
            var keyboards = await _context.Keyboards
                .Where(k => k.Type == null ||
                            (!k.Type.Contains("Gaming") && !k.Type.Contains("Игровая")))
                .ToListAsync();
            return View(keyboards);
        }

        // GET: /Keyboards/GetKeyboard/{id} – JSON для модального окна
        [HttpGet]
        public async Task<IActionResult> GetKeyboard(int id)
        {
            var keyboard = await _context.Keyboards.FindAsync(id);
            if (keyboard == null) return NotFound();
            return Json(keyboard);
        }
    }
}