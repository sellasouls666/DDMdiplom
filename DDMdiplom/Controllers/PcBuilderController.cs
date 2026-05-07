using Microsoft.AspNetCore.Mvc;

namespace DDMdiplom.Controllers
{
    public class PcBuilderController : Controller
    {
        [Route("PcBuilder")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
