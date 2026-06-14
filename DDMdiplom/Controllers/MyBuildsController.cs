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
                var priorityOrder = new List<string> {
                    "Процессор", "Материнская плата", "Видеокарта", "Оперативная память",
                    "Накопитель", "Блок питания", "Корпус", "Система охлаждения",
                    "Операционная система", "Монитор", "Источник бесперебойного питания",
                    "Клавиатура", "Мышь"
                };

                var sortedItems = build.Items
                    .OrderBy(item => {
                        int idx = priorityOrder.IndexOf(item.ComponentType);
                        return idx == -1 ? int.MaxValue : idx;
                    })
                    .ToList();

                decimal totalPrice = 0;
                var allPreviews = new List<ComponentPreview>();

                foreach (var item in sortedItems)
                {
                    var preview = await GetComponentPreviewAsync(item.ComponentType, item.ComponentId);
                    if (preview != null)
                    {
                        allPreviews.Add(preview);
                        totalPrice += preview.Price;
                    }
                }

                var previewsForCard = allPreviews.Take(3).ToList();

                model.Add(new BuildSummaryViewModel
                {
                    Id = build.Id,
                    Name = build.Name,
                    ComponentCount = build.Items.Count,
                    TotalPrice = totalPrice,
                    Previews = previewsForCard,
                    UpdatedAt = build.UpdatedAt
                });
            }

            return View(model);
        }

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

        /// <summary>
        /// Возвращает объект ComponentPreview для заданного компонента: имя, картинку, тип, краткие характеристики и цену.
        /// </summary>
        private async Task<ComponentPreview?> GetComponentPreviewAsync(string type, int id)
        {
            try
            {
                switch (type)
                {
                    case "Процессор":
                        var cpu = await _context.Processors.FindAsync(id);
                        if (cpu == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{cpu.Brand} {cpu.Name}",
                            ImageUrl = cpu.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{cpu.Cores} ядер / {cpu.Threads} потоков, {cpu.OperatingFrequency} ГГц, TDP {cpu.ThermalDesignPower} Вт",
                            Price = cpu.Price
                        };

                    case "Видеокарта":
                        var gpu = await _context.GraphicsCards.FindAsync(id);
                        if (gpu == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{gpu.Brand} {gpu.Model}",
                            ImageUrl = gpu.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{gpu.MemorySize} ГБ {gpu.MemoryType}, {gpu.ThermalDesignPower} Вт, {gpu.Cooler}",
                            Price = gpu.Price
                        };

                    case "Материнская плата":
                        var mb = await _context.Motherboards.FindAsync(id);
                        if (mb == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{mb.Brand} {mb.Model}",
                            ImageUrl = mb.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{mb.FormFactor}, {mb.Chipset}, сокет {mb.CpuSocketType}, {mb.MemorySlots} слота {mb.MemoryStandard}",
                            Price = mb.Price
                        };

                    case "Оперативная память":
                        var ram = await _context.Memories.FindAsync(id);
                        if (ram == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{ram.Brand} {ram.Model}",
                            ImageUrl = ram.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{ram.Capacity}, {ram.Speed}, CAS {ram.CasLatency}, {ram.Voltage} В",
                            Price = ram.Price
                        };

                    case "Накопитель":
                        var storage = await _context.Storages.FindAsync(id);
                        if (storage == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{storage.Brand} {storage.Model}",
                            ImageUrl = storage.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{storage.Capacity}, {storage.Interface}, {storage.FormFactor}",
                            Price = storage.Price
                        };

                    case "Блок питания":
                        var psu = await _context.PowerSupplies.FindAsync(id);
                        if (psu == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{psu.Brand} {psu.Model}",
                            ImageUrl = psu.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{psu.MaximumPower} Вт, {psu.EnergyEfficient}, {psu.Modular}",
                            Price = psu.Price
                        };

                    case "Корпус":
                        var pcCase = await _context.Cases.FindAsync(id);
                        if (pcCase == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{pcCase.Brand} {pcCase.Model}",
                            ImageUrl = pcCase.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{pcCase.Type}, {pcCase.Color}, макс. GPU {pcCase.MaxGpuLength} мм",
                            Price = pcCase.Price
                        };

                    case "Система охлаждения":
                        // Пробуем воздушное, потом жидкостное
                        var air = await _context.CpuCoolers.FindAsync(id);
                        if (air != null)
                        {
                            return new ComponentPreview
                            {
                                Name = $"{air.Brand} {air.Model}",
                                ImageUrl = air.ImageUrl ?? "/images/placeholder.png",
                                Type = type,
                                ShortSpecs = $"Воздушное, {air.FanSize}, {air.Rpm} RPM, {air.NoiseLevel} дБА",
                                Price = air.Price
                            };
                        }
                        var water = await _context.WaterCoolers.FindAsync(id);
                        if (water != null)
                        {
                            return new ComponentPreview
                            {
                                Name = $"{water.Brand} {water.Model}",
                                ImageUrl = water.ImageUrl ?? "/images/placeholder.png",
                                Type = type,
                                ShortSpecs = $"Жидкостное, {water.RadiatorSize}, {water.FanCount}x{water.FanSize} мм",
                                Price = water.Price
                            };
                        }
                        return null;

                    case "Операционная система":
                        var os = await _context.OperatingSystems.FindAsync(id);
                        if (os == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{os.Brand} {os.Name}",
                            ImageUrl = os.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{os.OperatingSystems}, {os.BitVersion}, {os.Packaging}",
                            Price = os.Price
                        };

                    case "Монитор":
                        var monitor = await _context.Monitors.FindAsync(id);
                        if (monitor == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{monitor.Brand} {monitor.Model}",
                            ImageUrl = monitor.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{monitor.ScreenSize}, {monitor.Resolution}, {monitor.RefreshRate} Гц, {monitor.Panel}",
                            Price = monitor.Price
                        };

                    case "Источник бесперебойного питания":
                        var ups = await _context.UpsDevices.FindAsync(id);
                        if (ups == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{ups.Brand} {ups.Model}",
                            ImageUrl = ups.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{ups.Watts} Вт / {ups.VaRating} VA, {ups.BatteryType}",
                            Price = ups.Price
                        };

                    case "Клавиатура":
                        var kb = await _context.Keyboards.FindAsync(id);
                        if (kb == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{kb.Brand} {kb.Name ?? kb.Model}",
                            ImageUrl = kb.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{(kb.IsMechanical == true ? "Механическая" : "Мембранная")}, {kb.KeySwitchType}, {kb.ConnectionType}",
                            Price = kb.Price
                        };

                    case "Мышь":
                        var mouse = await _context.Mice.FindAsync(id);
                        if (mouse == null) return null;
                        return new ComponentPreview
                        {
                            Name = $"{mouse.Brand} {mouse.Name}",
                            ImageUrl = mouse.ImageUrl ?? "/images/placeholder.png",
                            Type = type,
                            ShortSpecs = $"{mouse.MaxDpi} DPI, {mouse.TrackingMethod}, {mouse.ConnectionType}",
                            Price = mouse.Price
                        };

                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        // GET: /MyBuilds/GetBuildDetails/5
        [HttpGet]
        public async Task<IActionResult> GetBuildDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var build = await _context.Builds
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (build == null)
                return NotFound();

            var components = new List<object>();
            foreach (var item in build.Items)
            {
                var preview = await GetComponentPreviewAsync(item.ComponentType, item.ComponentId);
                if (preview != null)
                {
                    components.Add(new
                    {
                        name = preview.Name,
                        type = preview.Type,
                        shortSpecs = preview.ShortSpecs,
                        price = preview.Price,
                        imageUrl = preview.ImageUrl
                    });
                }
            }
            return Ok(components);
        }
    }
}