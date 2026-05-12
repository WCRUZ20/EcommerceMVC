using Ecommerce.Models.Identity;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[Authorize]
public class SettingsController(UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        return View(new UserProfileViewModel
        {
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            SecondLastName = user.SecondLastName,
            TipoDoc = user.TipoDoc,
            NumDocumento = user.NumDocumento,
            PhoneNumber = user.PhoneNumber ?? "No registrado",
            CreatedAtUtc = user.CreatedAtUtc,
            LastLoginAtUtc = user.LastLoginAtUtc
        });
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            TempData["StatusMessage"] = "Tu contraseña fue actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
}
