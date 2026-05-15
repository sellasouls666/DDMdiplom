using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DDMdiplom.Data;
using DDMdiplom.Models;

namespace DDMdiplom.Controllers
{
    public class GraphicsCardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GraphicsCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _context.GraphicsCards.ToListAsync();
            return View(cards);
        }

        [HttpGet]
        public async Task<IActionResult> GetGraphicsCard(int id)
        {
            var card = await _context.GraphicsCards.FindAsync(id);
            if (card == null) return NotFound();
            return Ok(card);
        }
    }
}