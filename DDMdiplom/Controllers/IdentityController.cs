using Microsoft.AspNetCore.Mvc;

namespace DDMdiplom.Controllers
{
    public class IdentityController : Controller
    {
        // GET: отображает страницу входа
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        // GET: отображает страницу регистрации
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
    }
}
