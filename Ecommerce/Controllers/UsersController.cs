using Ecommerce.Models.Identity;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = userManager.Users
            .OrderBy(user => user.UserName)
            .ToList();

        var model = new List<UserListItemViewModel>();
        foreach (var user in users)
        {
            model.Add(new UserListItemViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = FormatFullName(user),
                TipoDoc = user.TipoDoc,
                NumDocumento = user.NumDocumento,
                CreatedAtUtc = user.CreatedAtUtc,
                LastLoginAtUtc = user.LastLoginAtUtc,
                Roles = (await userManager.GetRolesAsync(user)).ToList()
            });
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateUserViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.UserName.Trim(),
            Email = model.Email.Trim(),
            FirstName = model.FirstName.Trim(),
            MiddleName = string.IsNullOrWhiteSpace(model.MiddleName) ? null : model.MiddleName.Trim(),
            LastName = model.LastName.Trim(),
            SecondLastName = string.IsNullOrWhiteSpace(model.SecondLastName) ? null : model.SecondLastName.Trim(),
            TipoDoc = model.TipoDoc.Trim(),
            NumDocumento = model.NumDocumento.Trim(),
            EmailConfirmed = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(user, model.Password);
        if (!createResult.Succeeded)
        {
            AddIdentityErrors(createResult.Errors);
            return View(model);
        }

        var role = model.Role.Trim();
        if (!string.IsNullOrWhiteSpace(role))
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    AddIdentityErrors(roleResult.Errors);
                    return View(model);
                }
            }

            var addRoleResult = await userManager.AddToRoleAsync(user, role);
            if (!addRoleResult.Succeeded)
            {
                AddIdentityErrors(addRoleResult.Errors);
                return View(model);
            }
        }

        TempData["StatusMessage"] = "Usuario creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private static string FormatFullName(ApplicationUser user)
    {
        var nameParts = new[]
        {
            user.FirstName,
            user.MiddleName,
            user.LastName,
            user.SecondLastName
        };

        return string.Join(" ", nameParts.Where(part => !string.IsNullOrWhiteSpace(part)));
    }

    private void AddIdentityErrors(IEnumerable<IdentityError> errors)
    {
        foreach (var error in errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
