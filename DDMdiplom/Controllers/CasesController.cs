using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CasesController : Controller
{
    private readonly ApplicationDbContext _context;
    public CasesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var cases = await _context.Cases.ToListAsync();
        return View(cases);
    }

    [HttpGet]
    public async Task<IActionResult> GetCase(int id)
    {
        var item = await _context.Cases.FindAsync(id);
        if (item == null) return NotFound();
        return Json(item);
    }
}