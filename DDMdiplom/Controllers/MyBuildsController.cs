using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DDMdiplom.Data;
using DDMdiplom.Models;
using DDMdiplom.ViewModels;

namespace DDMdiplom.Controllers
{
    public class MyBuildsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyBuildsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /MyBuilds
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var builds = await _context.Builds
                .Where(b => b.UserId == userId)
                .Include(b => b.Items)
                .OrderByDescending(b => b.UpdatedAt)
                .ToListAsync();

            var model = new List<BuildSummaryViewModel>();

            foreach (var build in builds)
            {
                // Порядок важности компонентов
                var priorityOrder = new List<string> {
                    "Процессор", "Материнская плата", "Видеокарта", "Оперативная память",
                    "Накопитель", "Блок питания", "Корпус", "Система охлаждения",
                    "Операционная система", "Монитор", "Источник бесперебойного питания",
                    "Клавиатура", "Мышь"
                };

                // Сортируем элементы сборки по приоритету
                var sortedItems = build.Items
                    .OrderBy(item => {
                        int idx = priorityOrder.IndexOf(item.ComponentType);
                        return idx == -1 ? int.MaxValue : idx;  // неизвестные типы – в конец
                    })
                    .ToList();

                decimal total = 0;
                var previews = new List<ComponentPreview>();
                int taken = 0;

                foreach (var item in sortedItems)
                {
                    if (taken >= 3) break;  // показываем не более трёх самых важных компонентов

                    var (name, price, img) = await GetComponentInfoAsync(item.ComponentType, item.ComponentId);
                    total += price;
                    previews.Add(new ComponentPreview { Name = name, ImageUrl = img });
                    taken++;
                }

                // Если после цикла остались неучтённые цены (для полной суммы), пройдите по оставшимся элементам
                foreach (var item in sortedItems.Skip(taken))
                {
                    var (_, price, _) = await GetComponentInfoAsync(item.ComponentType, item.ComponentId);
                    total += price;
                }

                model.Add(new BuildSummaryViewModel
                {
                    Id = build.Id,
                    Name = build.Name,
                    ComponentCount = build.Items.Count,
                    TotalPrice = total,
                    Previews = previews
                });
            }

            return View(model);
        }

        // POST: /MyBuilds/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var build = await _context.Builds
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (build != null)
            {
                _context.BuildItems.RemoveRange(build.Items);
                _context.Builds.Remove(build);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

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
                        var ram = await _context.Memories.FindAsync(id);  // ← Memories, не Memory
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
                        var ups = await _context.UpsDevices.FindAsync(id);  // ← UpsDevices, не Ups
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
    }
}