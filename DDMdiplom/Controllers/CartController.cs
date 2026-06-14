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
                .Include(ci => ci.Build)
                .ThenInclude(b => b.Items)
                .OrderByDescending(ci => ci.AddedAt)
                .ToListAsync();

            var model = new List<CartItemViewModel>();

            foreach (var cartItem in items)
            {
                var build = cartItem.Build;
                if (build == null) continue;

                decimal total = 0;
                var previews = new List<ComponentPreview>();

                foreach (var item in build.Items)
                {
                    var (name, price, img) = await GetComponentInfoAsync(item.ComponentType, item.ComponentId);
                    total += price;
                    previews.Add(new ComponentPreview { Name = name, ImageUrl = img });
                }

                model.Add(new CartItemViewModel
                {
                    Id = cartItem.Id,
                    Name = cartItem.Name,
                    ComponentCount = build.Items.Count,
                    TotalPrice = total,
                    Previews = previews,
                    Quantity = cartItem.Quantity,
                    AddedAt = cartItem.AddedAt
                });
            }

            return View(model);
        }

        // Скопированный метод из MyBuildsController
        private async Task<(string name, decimal price, string image)> GetComponentInfoAsync(string type, int id)
        {
            string name = type;
            decimal price = 0;
            string image = "/images/placeholder.png";

            try
            {
                switch (type)
                {
                    case "Процессор":
                        var cpu = await _context.Processors.FindAsync(id);
                        if (cpu != null) { name = $"{cpu.Brand} {cpu.Name}"; price = cpu.Price; image = cpu.ImageUrl ?? image; }
                        break;
                    case "Видеокарта":
                        var gpu = await _context.GraphicsCards.FindAsync(id);
                        if (gpu != null) { name = $"{gpu.Brand} {gpu.Model}"; price = gpu.Price; image = gpu.ImageUrl ?? image; }
                        break;
                    case "Материнская плата":
                        var mb = await _context.Motherboards.FindAsync(id);
                        if (mb != null) { name = $"{mb.Brand} {mb.Model}"; price = mb.Price; image = mb.ImageUrl ?? image; }
                        break;
                    case "Оперативная память":
                        var ram = await _context.Memories.FindAsync(id);
                        if (ram != null) { name = $"{ram.Brand} {ram.Model}"; price = ram.Price; image = ram.ImageUrl ?? image; }
                        break;
                    case "Накопитель":
                        var storage = await _context.Storages.FindAsync(id);
                        if (storage != null) { name = $"{storage.Brand} {storage.Model}"; price = storage.Price; image = storage.ImageUrl ?? image; }
                        break;
                    case "Блок питания":
                        var psu = await _context.PowerSupplies.FindAsync(id);
                        if (psu != null) { name = $"{psu.Brand} {psu.Model}"; price = psu.Price; image = psu.ImageUrl ?? image; }
                        break;
                    case "Корпус":
                        var pcCase = await _context.Cases.FindAsync(id);
                        if (pcCase != null) { name = $"{pcCase.Brand} {pcCase.Model}"; price = pcCase.Price; image = pcCase.ImageUrl ?? image; }
                        break;
                    case "Система охлаждения":
                        var air = await _context.CpuCoolers.FindAsync(id);
                        if (air != null) { name = $"{air.Brand} {air.Model}"; price = air.Price; image = air.ImageUrl ?? image; }
                        else
                        {
                            var water = await _context.WaterCoolers.FindAsync(id);
                            if (water != null) { name = $"{water.Brand} {water.Model}"; price = water.Price; image = water.ImageUrl ?? image; }
                        }
                        break;
                    case "Операционная система":
                        var os = await _context.OperatingSystems.FindAsync(id);
                        if (os != null) { name = $"{os.Brand} {os.Name}"; price = os.Price; image = os.ImageUrl ?? image; }
                        break;
                    case "Монитор":
                        var monitor = await _context.Monitors.FindAsync(id);
                        if (monitor != null) { name = $"{monitor.Brand} {monitor.Model}"; price = monitor.Price; image = monitor.ImageUrl ?? image; }
                        break;
                    case "Источник бесперебойного питания":
                        var ups = await _context.UpsDevices.FindAsync(id);
                        if (ups != null) { name = $"{ups.Brand} {ups.Model}"; price = ups.Price; image = ups.ImageUrl ?? image; }
                        break;
                    case "Клавиатура":
                        var kb = await _context.Keyboards.FindAsync(id);
                        if (kb != null) { name = $"{kb.Brand} {kb.Name ?? kb.Model}"; price = kb.Price; image = kb.ImageUrl ?? image; }
                        break;
                    case "Мышь":
                        var mouse = await _context.Mice.FindAsync(id);
                        if (mouse != null) { name = $"{mouse.Brand} {mouse.Name}"; price = mouse.Price; image = mouse.ImageUrl ?? image; }
                        break;
                }
            }
            catch { /* ignore */ }

            return (name, price, image);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartAddRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            if (!request.BuildId.HasValue)
                return BadRequest("Не указана сборка");

            var build = await _context.Builds
                .FirstOrDefaultAsync(b => b.Id == request.BuildId && b.UserId == userId);
            if (build == null) return NotFound("Сборка не найдена");

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.BuildId == request.BuildId);

            if (existing != null)
            {
                if (existing.Quantity < 10) existing.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    Name = request.Name ?? build.Name,
                    BuildId = build.Id,
                    Quantity = 1
                });
            }

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
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == id && ci.UserId == userId);
            if (item != null && item.Quantity < 10)
            {
                item.Quantity++;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var item = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == id && ci.UserId == userId);
            if (item != null)
            {
                if (item.Quantity > 1)
                    item.Quantity--;
                else
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