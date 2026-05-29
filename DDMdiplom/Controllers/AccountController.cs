using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DDMdiplom.Models;
using DDMdiplom.ViewModels;

namespace DDMdiplom.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AccountController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        // GET: Account/Settings
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var model = new ProfileViewModel
            {
                FullName = user.Name,
                Phone = user.PhoneNumber,
                Address = user.Address
            };

            ViewBag.AvatarUrl = user.AvatarUrl;   // фактический URL или null
            ViewBag.HasAvatar = !string.IsNullOrEmpty(user.AvatarUrl);

            return View(model);
        }

        // POST: Account/Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (!ModelState.IsValid)
            {
                ViewBag.AvatarUrl = user.AvatarUrl;
                ViewBag.HasAvatar = !string.IsNullOrEmpty(user.AvatarUrl);
                return View(model);
            }

            user.Name = model.FullName;
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;
            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = "Профиль обновлён";
            return RedirectToAction("Settings");
        }

        // POST: Account/UploadAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || avatar == null || avatar.Length == 0)
                return RedirectToAction("Settings");

            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "avatars");
            Directory.CreateDirectory(uploadsDir);

            // Удаляем старый файл
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldPath = Path.Combine(_env.WebRootPath, user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var ext = Path.GetExtension(avatar.FileName);
            var fileName = $"{user.Id}_{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
                await avatar.CopyToAsync(stream);

            user.AvatarUrl = $"/uploads/avatars/{fileName}";
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Settings");
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Settings");

            if (!ModelState.IsValid)
            {
                TempData["PasswordError"] = "Проверьте правильность заполнения полей";
                return RedirectToAction("Settings");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Пароль успешно изменён";
            }
            else
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                TempData["PasswordError"] = errors;
            }

            return RedirectToAction("Settings");
        }

        // POST: Account/DeleteAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAvatar()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Settings");

            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var path = Path.Combine(_env.WebRootPath, user.AvatarUrl.TrimStart('/'));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            user.AvatarUrl = null;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Settings");
        }
    }
}