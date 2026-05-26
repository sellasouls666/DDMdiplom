using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DDMdiplom.Data;
using DDMdiplom.Models;
using DDMdiplom.ViewModels;

namespace DDMdiplom.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var items = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .OrderByDescending(ci => ci.AddedAt)
                .ToListAsync();

            var model = items.Select(item =>
            {
                var components = JsonSerializer.Deserialize<List<BuildComponent>>(item.ComponentsJson)
                                 ?? new List<BuildComponent>();
                decimal total = 0;
                var previews = new List<ComponentPreview>();
                int idx = 0;
                foreach (var comp in components)
                {
                    total += comp.Price;
                    if (idx < 3)
                        previews.Add(new ComponentPreview { Name = comp.Name, ImageUrl = comp.ImageUrl ?? "/images/placeholder.png" });
                    idx++;
                }
                return new CartItemViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ComponentCount = components.Count,
                    TotalPrice = total,
                    Previews = previews,
                    AddedAt = item.AddedAt
                };
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartAddRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            if (request.Components == null || request.Components.Count == 0)
                return BadRequest("Сборка пуста");

            var cartItem = new CartItem
            {
                UserId = userId,
                Name = request.Name ?? "Моя конфигурация",
                ComponentsJson = JsonSerializer.Serialize(request.Components),
                AddedAt = DateTime.UtcNow
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == id && ci.UserId == userId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _context.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}