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

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Списываем количество комплектующих
                foreach (var cartItem in cartItems)
                {
                    var build = cartItem.Build;
                    if (build?.Items == null) continue;

                    foreach (var buildItem in build.Items)
                    {
                        int quantityToDecrease = cartItem.Quantity;
                        if (buildItem.ComponentType == "Оперативная память")
                        {
                            quantityToDecrease *= buildItem.ModuleCount;
                        }

                        var (success, errorMsg) = await DecreaseComponentQuantity(buildItem.ComponentType, buildItem.ComponentId, quantityToDecrease);
                        if (!success)
                        {
                            throw new Exception(errorMsg);
                        }
                    }
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

                await transaction.CommitAsync();
                return RedirectToAction("Success", new { id = order.Id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Ошибка при оформлении заказа: {ex.Message}");
                model.CartItems = await BuildCartItemSummaries(cartItems);
                model.TotalPrice = model.CartItems.Sum(c => c.Price * c.Quantity);
                return View(model);
            }
        }

        private async Task<(bool success, string errorMessage)> DecreaseComponentQuantity(string componentType, int componentId, int quantity)
        {
            try
            {
                switch (componentType)
                {
                    case "Процессор":
                        var cpu = await _context.Processors.FindAsync(componentId);
                        if (cpu == null) return (false, $"Процессор с ID {componentId} не найден");
                        if (cpu.Quantity < quantity) return (false, $"Процессор {cpu.Brand} {cpu.Name} в наличии {cpu.Quantity} шт., требуется {quantity} шт.");
                        cpu.Quantity -= quantity;
                        break;
                    case "Видеокарта":
                        var gpu = await _context.GraphicsCards.FindAsync(componentId);
                        if (gpu == null) return (false, $"Видеокарта с ID {componentId} не найдена");
                        if (gpu.Quantity < quantity) return (false, $"Видеокарта {gpu.Brand} {gpu.Model} в наличии {gpu.Quantity} шт., требуется {quantity} шт.");
                        gpu.Quantity -= quantity;
                        break;
                    case "Материнская плата":
                        var mb = await _context.Motherboards.FindAsync(componentId);
                        if (mb == null) return (false, $"Материнская плата с ID {componentId} не найдена");
                        if (mb.Quantity < quantity) return (false, $"Материнская плата {mb.Brand} {mb.Model} в наличии {mb.Quantity} шт., требуется {quantity} шт.");
                        mb.Quantity -= quantity;
                        break;
                    case "Оперативная память":
                        var ram = await _context.Memories.FindAsync(componentId);
                        if (ram == null) return (false, $"Модуль памяти с ID {componentId} не найден");
                        if (ram.Quantity < quantity) return (false, $"Модуль памяти {ram.Brand} {ram.Model} в наличии {ram.Quantity} шт., требуется {quantity} шт.");
                        ram.Quantity -= quantity;
                        break;
                    case "Накопитель":
                        var storage = await _context.Storages.FindAsync(componentId);
                        if (storage == null) return (false, $"Накопитель с ID {componentId} не найден");
                        if (storage.Quantity < quantity) return (false, $"Накопитель {storage.Brand} {storage.Model} в наличии {storage.Quantity} шт., требуется {quantity} шт.");
                        storage.Quantity -= quantity;
                        break;
                    case "Блок питания":
                        var psu = await _context.PowerSupplies.FindAsync(componentId);
                        if (psu == null) return (false, $"Блок питания с ID {componentId} не найден");
                        if (psu.Quantity < quantity) return (false, $"Блок питания {psu.Brand} {psu.Model} в наличии {psu.Quantity} шт., требуется {quantity} шт.");
                        psu.Quantity -= quantity;
                        break;
                    case "Корпус":
                        var pcCase = await _context.Cases.FindAsync(componentId);
                        if (pcCase == null) return (false, $"Корпус с ID {componentId} не найден");
                        if (pcCase.Quantity < quantity) return (false, $"Корпус {pcCase.Brand} {pcCase.Model} в наличии {pcCase.Quantity} шт., требуется {quantity} шт.");
                        pcCase.Quantity -= quantity;
                        break;
                    case "Система охлаждения":
                        var air = await _context.CpuCoolers.FindAsync(componentId);
                        if (air != null)
                        {
                            if (air.Quantity < quantity) return (false, $"Кулер {air.Brand} {air.Model} в наличии {air.Quantity} шт., требуется {quantity} шт.");
                            air.Quantity -= quantity;
                            break;
                        }
                        var water = await _context.WaterCoolers.FindAsync(componentId);
                        if (water != null)
                        {
                            if (water.Quantity < quantity) return (false, $"СЖО {water.Brand} {water.Model} в наличии {water.Quantity} шт., требуется {quantity} шт.");
                            water.Quantity -= quantity;
                            break;
                        }
                        return (false, $"Система охлаждения с ID {componentId} не найдена");
                    case "Операционная система":
                        var os = await _context.OperatingSystems.FindAsync(componentId);
                        if (os == null) return (false, $"ОС с ID {componentId} не найдена");
                        if (os.Quantity < quantity) return (false, $"ОС {os.Brand} {os.Name} в наличии {os.Quantity} шт., требуется {quantity} шт.");
                        os.Quantity -= quantity;
                        break;
                    case "Монитор":
                        var monitor = await _context.Monitors.FindAsync(componentId);
                        if (monitor == null) return (false, $"Монитор с ID {componentId} не найден");
                        if (monitor.Quantity < quantity) return (false, $"Монитор {monitor.Brand} {monitor.Model} в наличии {monitor.Quantity} шт., требуется {quantity} шт.");
                        monitor.Quantity -= quantity;
                        break;
                    case "Источник бесперебойного питания":
                        var ups = await _context.UpsDevices.FindAsync(componentId);
                        if (ups == null) return (false, $"ИБП с ID {componentId} не найден");
                        if (ups.Quantity < quantity) return (false, $"ИБП {ups.Brand} {ups.Model} в наличии {ups.Quantity} шт., требуется {quantity} шт.");
                        ups.Quantity -= quantity;
                        break;
                    case "Клавиатура":
                        var kb = await _context.Keyboards.FindAsync(componentId);
                        if (kb == null) return (false, $"Клавиатура с ID {componentId} не найдена");
                        if (kb.Quantity < quantity) return (false, $"Клавиатура {kb.Brand} {kb.Name ?? kb.Model} в наличии {kb.Quantity} шт., требуется {quantity} шт.");
                        kb.Quantity -= quantity;
                        break;
                    case "Мышь":
                        var mouse = await _context.Mice.FindAsync(componentId);
                        if (mouse == null) return (false, $"Мышь с ID {componentId} не найдена");
                        if (mouse.Quantity < quantity) return (false, $"Мышь {mouse.Brand} {mouse.Name} в наличии {mouse.Quantity} шт., требуется {quantity} шт.");
                        mouse.Quantity -= quantity;
                        break;
                    default:
                        return (false, $"Неизвестный тип компонента: {componentType}");
                }
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при списании {componentType}: {ex.Message}");
            }
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

        // GET: /Orders
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Build)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Build)
                        .ThenInclude(b => b.Items)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null) return NotFound();

            // Разрешаем отмену только для определённых статусов
            if (order.Status != "Новый" && order.Status != "В обработке")
            {
                TempData["Error"] = "Заказ нельзя отменить, так как он уже в статусе: " + order.Status;
                return RedirectToAction(nameof(Index));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Возвращаем все комплектующие на склад
                foreach (var orderItem in order.OrderItems)
                {
                    var build = orderItem.Build;
                    if (build?.Items == null) continue;

                    foreach (var buildItem in build.Items)
                    {
                        int quantityToReturn = orderItem.Quantity;
                        if (buildItem.ComponentType == "Оперативная память")
                        {
                            quantityToReturn *= buildItem.ModuleCount;
                        }

                        bool success = await ReturnComponentQuantity(buildItem.ComponentType, buildItem.ComponentId, quantityToReturn);
                        if (!success)
                        {
                            throw new Exception($"Ошибка при возврате компонента {buildItem.ComponentType} ID {buildItem.ComponentId}");
                        }
                    }
                }

                order.Status = "Отменён";
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Заказ успешно отменён.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = $"Ошибка при отмене заказа: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ReturnComponentQuantity(string componentType, int componentId, int quantity)
        {
            try
            {
                switch (componentType)
                {
                    case "Процессор":
                        var cpu = await _context.Processors.FindAsync(componentId);
                        if (cpu != null) cpu.Quantity += quantity;
                        break;
                    case "Видеокарта":
                        var gpu = await _context.GraphicsCards.FindAsync(componentId);
                        if (gpu != null) gpu.Quantity += quantity;
                        break;
                    case "Материнская плата":
                        var mb = await _context.Motherboards.FindAsync(componentId);
                        if (mb != null) mb.Quantity += quantity;
                        break;
                    case "Оперативная память":
                        var ram = await _context.Memories.FindAsync(componentId);
                        if (ram != null) ram.Quantity += quantity;
                        break;
                    case "Накопитель":
                        var storage = await _context.Storages.FindAsync(componentId);
                        if (storage != null) storage.Quantity += quantity;
                        break;
                    case "Блок питания":
                        var psu = await _context.PowerSupplies.FindAsync(componentId);
                        if (psu != null) psu.Quantity += quantity;
                        break;
                    case "Корпус":
                        var pcCase = await _context.Cases.FindAsync(componentId);
                        if (pcCase != null) pcCase.Quantity += quantity;
                        break;
                    case "Система охлаждения":
                        var air = await _context.CpuCoolers.FindAsync(componentId);
                        if (air != null) air.Quantity += quantity;
                        else
                        {
                            var water = await _context.WaterCoolers.FindAsync(componentId);
                            if (water != null) water.Quantity += quantity;
                        }
                        break;
                    case "Операционная система":
                        var os = await _context.OperatingSystems.FindAsync(componentId);
                        if (os != null) os.Quantity += quantity;
                        break;
                    case "Монитор":
                        var monitor = await _context.Monitors.FindAsync(componentId);
                        if (monitor != null) monitor.Quantity += quantity;
                        break;
                    case "Источник бесперебойного питания":
                        var ups = await _context.UpsDevices.FindAsync(componentId);
                        if (ups != null) ups.Quantity += quantity;
                        break;
                    case "Клавиатура":
                        var kb = await _context.Keyboards.FindAsync(componentId);
                        if (kb != null) kb.Quantity += quantity;
                        break;
                    case "Мышь":
                        var mouse = await _context.Mice.FindAsync(componentId);
                        if (mouse != null) mouse.Quantity += quantity;
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}