using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class PowerSuppliesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PowerSuppliesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /PowerSupplies
        public async Task<IActionResult> Index()
        {
            var powerSupplies = await _context.PowerSupplies.ToListAsync();
            return View(powerSupplies);
        }

        // GET: /PowerSupplies/GetPowerSupply/{id}
        [HttpGet]
        public async Task<IActionResult> GetPowerSupply(int id)
        {
            var psu = await _context.PowerSupplies.FindAsync(id);
            if (psu == null)
                return NotFound();
            return Json(psu);
        }
    }
}