using DDMdiplom.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DDMdiplom.ViewModels;

namespace DDMdiplom.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; //сервис для работы с пользователями
        private readonly SignInManager<ApplicationUser> _signInManager; //сервис для аутентификации

        public IdentityController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Создаём нового пользователя
                var user = new ApplicationUser
                {
                    UserName = model.Email,     // используем email как логин
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    Name = model.FullName,       // дополнительное поле
                    CreatedAt = DateTime.UtcNow,
                };

                var result = await _userManager.CreateAsync(user, model.Password); //создаём нового пользователя

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false); //после регистрации остаёмся в аккаунте
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var firstError = result.Errors.FirstOrDefault(); //т.к. в моей валидации все ошибки с паролем имеют одинаковый текст, отображаем только первую ошибку
                    if (firstError != null)
                    {
                        ModelState.AddModelError(string.Empty, firstError.Description);
                    }
                }
            }

            // Если что-то не так – возвращаем ту же страницу с ошибками
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CheckUserExists(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Json(new { exists = false });
            var user = await _userManager.FindByEmailAsync(email);
            return Json(new { exists = user != null });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Мы не нашли совпадений, пожалуйста, попробуйте еще раз.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Адрес электронной почты и пароль не совпадают, пожалуйста, повторите попытку.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

