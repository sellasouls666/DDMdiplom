using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class MotherboardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotherboardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var motherboards = await _context.Motherboards.ToListAsync();
            return View(motherboards);
        }

        [HttpGet]
        public async Task<IActionResult> GetMotherboard(int id)
        {
            var motherboard = await _context.Motherboards.FindAsync(id);
            if (motherboard == null) return NotFound();
            return Ok(motherboard);
        }
    }
}