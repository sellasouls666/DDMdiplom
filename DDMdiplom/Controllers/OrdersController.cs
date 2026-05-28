using DDMdiplom.Data;
using DDMdiplom.Models;
using DDMdiplom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DDMdiplom.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var cartItems = await _context.CartItems
                .Include(c => c.Build)
                .ThenInclude(b => b.Items)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Ваша корзина пуста.";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel();
            foreach (var cartItem in cartItems)
            {
                decimal price = await CalculateBuildPrice(cartItem.Build);
                model.CartItems.Add(new CartItemSummaryViewModel
                {
                    BuildId = cartItem.BuildId,
                    BuildName = cartItem.Build?.Name ?? "Безымянная сборка",
                    Quantity = cartItem.Quantity,
                    Price = price
                });
            }
            model.TotalPrice = model.CartItems.Sum(c => c.Price * c.Quantity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var cartItems = await _context.CartItems
                .Include(c => c.Build)
                .ThenInclude(b => b.Items)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                ModelState.AddModelError("", "Корзина пуста.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                model.CartItems = await BuildCartItemSummaries(cartItems);
                model.TotalPrice = model.CartItems.Sum(c => c.Price * c.Quantity);
                return View(model);
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Новый",
                CustomerName = model.CustomerName,
                Phone = model.Phone,
                Address = model.Address,
                Comment = model.Comment
            };

            decimal totalOrderPrice = 0;
            foreach (var cartItem in cartItems)
            {
                decimal buildPrice = await CalculateBuildPrice(cartItem.Build);
                totalOrderPrice += buildPrice * cartItem.Quantity;
                order.OrderItems.Add(new OrderItem
                {
                    BuildId = cartItem.BuildId,
                    Quantity = cartItem.Quantity,
                    Price = buildPrice
                });
            }
            order.TotalPrice = totalOrderPrice;

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success", new { id = order.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetBuildComponents(int buildId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var build = await _context.Builds
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == buildId && b.UserId == userId);
            if (build == null) return NotFound();

            var components = new List<ComponentDetailViewModel>();
            foreach (var item in build.Items)
            {
                var info = await GetComponentInfoAsync(item.ComponentType, item.ComponentId);
                components.Add(new ComponentDetailViewModel { Name = info.name, ImageUrl = info.image });
            }
            return PartialView("_BuildComponentsPartial", components);
        }

        private async Task<decimal> CalculateBuildPrice(Build? build)
        {
            if (build?.Items == null) return 0;
            decimal total = 0;
            foreach (var item in build.Items)
            {
                decimal price = item.ComponentType switch
                {
                    "Процессор" => (await _context.Processors.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Видеокарта" => (await _context.GraphicsCards.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Материнская плата" => (await _context.Motherboards.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Оперативная память" => (await _context.Memories.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Накопитель" => (await _context.Storages.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Блок питания" => (await _context.PowerSupplies.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Корпус" => (await _context.Cases.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Система охлаждения" => (await _context.CpuCoolers.FindAsync(item.ComponentId))?.Price ??
                                           (await _context.WaterCoolers.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Операционная система" => (await _context.OperatingSystems.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Монитор" => (await _context.Monitors.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Источник бесперебойного питания" => (await _context.UpsDevices.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Клавиатура" => (await _context.Keyboards.FindAsync(item.ComponentId))?.Price ?? 0,
                    "Мышь" => (await _context.Mice.FindAsync(item.ComponentId))?.Price ?? 0,
                    _ => 0
                };
                total += item.ComponentType == "Оперативная память" ? price * item.ModuleCount : price;
            }
            return total;
        }

        private async Task<List<CartItemSummaryViewModel>> BuildCartItemSummaries(List<CartItem> cartItems)
        {
            var list = new List<CartItemSummaryViewModel>();
            foreach (var cartItem in cartItems)
            {
                decimal price = await CalculateBuildPrice(cartItem.Build);
                list.Add(new CartItemSummaryViewModel
                {
                    BuildId = cartItem.BuildId,
                    BuildName = cartItem.Build?.Name ?? "Безымянная сборка",
                    Quantity = cartItem.Quantity,
                    Price = price
                });
            }
            return list;
        }

        private async Task<(string name, string image)> GetComponentInfoAsync(string type, int id)
        {
            string name = type;
            string image = "/images/placeholder.png";
            try
            {
                switch (type)
                {
                    case "Процессор":
                        var cpu = await _context.Processors.FindAsync(id);
                        if (cpu != null) { name = $"{cpu.Brand} {cpu.Name}"; image = cpu.ImageUrl ?? image; }
                        break;
                    case "Видеокарта":
                        var gpu = await _context.GraphicsCards.FindAsync(id);
                        if (gpu != null) { name = $"{gpu.Brand} {gpu.Model}"; image = gpu.ImageUrl ?? image; }
                        break;
                    case "Материнская плата":
                        var mb = await _context.Motherboards.FindAsync(id);
                        if (mb != null) { name = $"{mb.Brand} {mb.Model}"; image = mb.ImageUrl ?? image; }
                        break;
                    case "Оперативная память":
                        var ram = await _context.Memories.FindAsync(id);
                        if (ram != null) { name = $"{ram.Brand} {ram.Model}"; image = ram.ImageUrl ?? image; }
                        break;
                    case "Накопитель":
                        var storage = await _context.Storages.FindAsync(id);
                        if (storage != null) { name = $"{storage.Brand} {storage.Model}"; image = storage.ImageUrl ?? image; }
                        break;
                    case "Блок питания":
                        var psu = await _context.PowerSupplies.FindAsync(id);
                        if (psu != null) { name = $"{psu.Brand} {psu.Model}"; image = psu.ImageUrl ?? image; }
                        break;
                    case "Корпус":
                        var pcCase = await _context.Cases.FindAsync(id);
                        if (pcCase != null) { name = $"{pcCase.Brand} {pcCase.Model}"; image = pcCase.ImageUrl ?? image; }
                        break;
                    case "Система охлаждения":
                        var air = await _context.CpuCoolers.FindAsync(id);
                        if (air != null) { name = $"{air.Brand} {air.Model}"; image = air.ImageUrl ?? image; }
                        else
                        {
                            var water = await _context.WaterCoolers.FindAsync(id);
                            if (water != null) { name = $"{water.Brand} {water.Model}"; image = water.ImageUrl ?? image; }
                        }
                        break;
                    case "Операционная система":
                        var os = await _context.OperatingSystems.FindAsync(id);
                        if (os != null) { name = $"{os.Brand} {os.Name}"; image = os.ImageUrl ?? image; }
                        break;
                    case "Монитор":
                        var monitor = await _context.Monitors.FindAsync(id);
                        if (monitor != null) { name = $"{monitor.Brand} {monitor.Model}"; image = monitor.ImageUrl ?? image; }
                        break;
                    case "Источник бесперебойного питания":
                        var ups = await _context.UpsDevices.FindAsync(id);
                        if (ups != null) { name = $"{ups.Brand} {ups.Model}"; image = ups.ImageUrl ?? image; }
                        break;
                    case "Клавиатура":
                        var kb = await _context.Keyboards.FindAsync(id);
                        if (kb != null) { name = $"{kb.Brand} {kb.Name ?? kb.Model}"; image = kb.ImageUrl ?? image; }
                        break;
                    case "Мышь":
                        var mouse = await _context.Mice.FindAsync(id);
                        if (mouse != null) { name = $"{mouse.Brand} {mouse.Name}"; image = mouse.ImageUrl ?? image; }
                        break;
                }
            }
            catch { }
            return (name, image);
        }

        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}