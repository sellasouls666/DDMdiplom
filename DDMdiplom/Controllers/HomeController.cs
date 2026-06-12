using DDMdiplom.Data;
using DDMdiplom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DDMdiplom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Получаем все системные сборки (IsSystem == true) с их элементами
            var builds = await _context.Builds
                .Where(b => b.IsSystem)
                .Include(b => b.Items)
                .ToListAsync();

            // Рассчитываем TotalPrice и ComponentCount для каждой сборки
            foreach (var build in builds)
            {
                decimal total = 0;
                foreach (var item in build.Items)
                {
                    decimal price = await CalculateComponentPrice(item.ComponentType, item.ComponentId);
                    total += item.ComponentType == "Оперативная память" ? price * item.ModuleCount : price;
                }
                build.TotalPrice = total;
                build.ComponentCount = build.Items.Count;
            }

            return View(builds);
        }

        private async Task<decimal> CalculateComponentPrice(string componentType, int componentId)
        {
            try
            {
                switch (componentType)
                {
                    case "Процессор":
                        var cpu = await _context.Processors.FindAsync(componentId);
                        return cpu?.Price ?? 0;
                    case "Видеокарта":
                        var gpu = await _context.GraphicsCards.FindAsync(componentId);
                        return gpu?.Price ?? 0;
                    case "Материнская плата":
                        var mb = await _context.Motherboards.FindAsync(componentId);
                        return mb?.Price ?? 0;
                    case "Оперативная память":
                        var ram = await _context.Memories.FindAsync(componentId);
                        return ram?.Price ?? 0;
                    case "Накопитель":
                        var storage = await _context.Storages.FindAsync(componentId);
                        return storage?.Price ?? 0;
                    case "Блок питания":
                        var psu = await _context.PowerSupplies.FindAsync(componentId);
                        return psu?.Price ?? 0;
                    case "Корпус":
                        var pcCase = await _context.Cases.FindAsync(componentId);
                        return pcCase?.Price ?? 0;
                    case "Система охлаждения":
                        var air = await _context.CpuCoolers.FindAsync(componentId);
                        if (air != null) return air.Price;
                        var water = await _context.WaterCoolers.FindAsync(componentId);
                        return water?.Price ?? 0;
                    case "Операционная система":
                        var os = await _context.OperatingSystems.FindAsync(componentId);
                        return os?.Price ?? 0;
                    case "Монитор":
                        var monitor = await _context.Monitors.FindAsync(componentId);
                        return monitor?.Price ?? 0;
                    case "Источник бесперебойного питания":
                        var ups = await _context.UpsDevices.FindAsync(componentId);
                        return ups?.Price ?? 0;
                    case "Клавиатура":
                        var kb = await _context.Keyboards.FindAsync(componentId);
                        return kb?.Price ?? 0;
                    case "Мышь":
                        var mouse = await _context.Mice.FindAsync(componentId);
                        return mouse?.Price ?? 0;
                    default:
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}