using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDMdiplom.Data;
using DDMdiplom.Models;

namespace DDMdiplom.Controllers
{
    public class ProcessorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var processors = await _context.Processors.ToListAsync();
            return View(processors);
        }

        [HttpGet]
        public async Task<IActionResult> GetProcessor(int id)
        {
            var processor = await _context.Processors.FindAsync(id);
            if (processor == null) return NotFound();
            return Ok(processor);
        }
    }
}