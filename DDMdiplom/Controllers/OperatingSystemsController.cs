using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class OperatingSystemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OperatingSystemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /OperatingSystems
        public async Task<IActionResult> Index()
        {
            var osList = await _context.OperatingSystems.ToListAsync();
            return View(osList);
        }

        // GET: /OperatingSystems/GetOperatingSystem/{id}
        [HttpGet]
        public async Task<IActionResult> GetOperatingSystem(int id)
        {
            var os = await _context.OperatingSystems.FindAsync(id);
            if (os == null) return NotFound();
            return Json(os);
        }
    }
}