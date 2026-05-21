using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDMdiplom.Controllers
{
    public class UpsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UpsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Ups
        public async Task<IActionResult> Index()
        {
            var devices = await _context.UpsDevices.ToListAsync();
            return View(devices);
        }

        // GET: /Ups/GetUps/{id} – JSON для модального окна
        [HttpGet]
        public async Task<IActionResult> GetUps(int id)
        {
            var device = await _context.UpsDevices.FindAsync(id);
            if (device == null) return NotFound();
            return Json(device);
        }
    }
}