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
                    Name = model.FullName       // дополнительное поле
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
    }
}

