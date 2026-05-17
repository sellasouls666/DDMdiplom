using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class MemoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var memories = await _context.Memories.ToListAsync();
            return View(memories);
        }

        [HttpGet]
        public async Task<IActionResult> GetMemory(int id)
        {
            var memory = await _context.Memories.FindAsync(id);
            if (memory == null) return NotFound();
            return Ok(memory);
        }
    }
}