using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Models.Identity;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : Controller
{
    private static readonly string[] AllowedRoles = ["User", "Admin"];

    public async Task<IActionResult> Index()
    {
        var users = await userManager.Users
            .Include(user => user.TipoDocumento)
            .OrderBy(user => user.UserName)
            .ToListAsync();

        var model = new List<UserListItemViewModel>();
        foreach (var user in users)
        {
            model.Add(new UserListItemViewModel
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = FormatFullName(user),
                TipoDoc = user.TipoDocumento?.Descripcion ?? user.TipoDoc,
                NumDocumento = user.NumDocumento,
                CreatedAtUtc = user.CreatedAtUtc,
                LastLoginAtUtc = user.LastLoginAtUtc,
                Roles = (await userManager.GetRolesAsync(user)).ToList()
            });
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateUserViewModel();
        await PopulateDocumentOptionsAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        var tipoDocumento = await ValidateDocumentAsync(model.TipoDocumentoId, model.NumDocumento);

        if (!ModelState.IsValid || tipoDocumento is null)
        {
            await PopulateDocumentOptionsAsync(model);
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
            TipoDocumentoId = tipoDocumento.Id,
            TipoDoc = tipoDocumento.Descripcion,
            NumDocumento = model.NumDocumento.Trim(),
            EmailConfirmed = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(user, model.Password);
        if (!createResult.Succeeded)
        {
            AddIdentityErrors(createResult.Errors);
            await PopulateDocumentOptionsAsync(model);
            return View(model);
        }

        var roleResult = await SetUserRoleAsync(user, model.Role);
        if (!roleResult.Succeeded)
        {
            AddIdentityErrors(roleResult.Errors);
            await PopulateDocumentOptionsAsync(model);
            return View(model);
        }

        TempData["StatusMessage"] = "Usuario creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        var user = await userManager.Users
            .Include(applicationUser => applicationUser.TipoDocumento)
            .FirstOrDefaultAsync(applicationUser => applicationUser.Id == id);

        if (user is null)
        {
            return NotFound();
        }

        var roles = await userManager.GetRolesAsync(user);
        var model = new EditUserViewModel
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            SecondLastName = user.SecondLastName,
            TipoDocumentoId = user.TipoDocumentoId,
            NumDocumento = user.NumDocumento,
            Role = roles.FirstOrDefault() ?? "User"
        };

        await PopulateDocumentOptionsAsync(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EditUserViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var tipoDocumento = await ValidateDocumentAsync(model.TipoDocumentoId, model.NumDocumento);
        if (!ModelState.IsValid || tipoDocumento is null)
        {
            await PopulateDocumentOptionsAsync(model);
            return View(model);
        }

        user.UserName = model.UserName.Trim();
        user.Email = model.Email.Trim();
        user.FirstName = model.FirstName.Trim();
        user.MiddleName = string.IsNullOrWhiteSpace(model.MiddleName) ? null : model.MiddleName.Trim();
        user.LastName = model.LastName.Trim();
        user.SecondLastName = string.IsNullOrWhiteSpace(model.SecondLastName) ? null : model.SecondLastName.Trim();
        user.TipoDocumentoId = tipoDocumento.Id;
        user.TipoDoc = tipoDocumento.Descripcion;
        user.NumDocumento = model.NumDocumento.Trim();

        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            AddIdentityErrors(updateResult.Errors);
            await PopulateDocumentOptionsAsync(model);
            return View(model);
        }

        var roleResult = await SetUserRoleAsync(user, model.Role);
        if (!roleResult.Succeeded)
        {
            AddIdentityErrors(roleResult.Errors);
            await PopulateDocumentOptionsAsync(model);
            return View(model);
        }

        TempData["StatusMessage"] = "Usuario actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<TipoDocumento?> ValidateDocumentAsync(int? tipoDocumentoId, string numDocumento)
    {
        if (tipoDocumentoId is null)
        {
            return null;
        }

        var tipoDocumento = await dbContext.TiposDocumento
            .AsNoTracking()
            .FirstOrDefaultAsync(documento => documento.Id == tipoDocumentoId.Value);

        if (tipoDocumento is null)
        {
            ModelState.AddModelError(nameof(CreateUserViewModel.TipoDocumentoId), "Selecciona un tipo de documento válido.");
            return null;
        }

        var trimmedDocument = numDocumento.Trim();
        if (tipoDocumento.CountValid is int countValid && trimmedDocument.Length != countValid)
        {
            ModelState.AddModelError(
                nameof(CreateUserViewModel.NumDocumento),
                $"El número de documento para {tipoDocumento.Descripcion} debe tener exactamente {countValid} caracteres.");
        }

        return tipoDocumento;
    }

    private async Task PopulateDocumentOptionsAsync(CreateUserViewModel model)
    {
        var tiposDocumento = await GetTiposDocumentoAsync();
        model.TipoDocumentoOptions = BuildDocumentOptions(tiposDocumento);
        model.CountValidByTipoDocumento = BuildCountValidMap(tiposDocumento);
    }

    private async Task PopulateDocumentOptionsAsync(EditUserViewModel model)
    {
        var tiposDocumento = await GetTiposDocumentoAsync();
        model.TipoDocumentoOptions = BuildDocumentOptions(tiposDocumento);
        model.CountValidByTipoDocumento = BuildCountValidMap(tiposDocumento);
    }

    private async Task<List<TipoDocumento>> GetTiposDocumentoAsync()
    {
        return await dbContext.TiposDocumento
            .AsNoTracking()
            .OrderBy(tipoDocumento => tipoDocumento.Descripcion)
            .ToListAsync();
    }

    private static List<SelectListItem> BuildDocumentOptions(IEnumerable<TipoDocumento> tiposDocumento)
    {
        return tiposDocumento
            .Select(tipoDocumento => new SelectListItem
            {
                Value = tipoDocumento.Id.ToString(),
                Text = tipoDocumento.CountValid is int countValid
                    ? $"{tipoDocumento.Descripcion} ({countValid} caracteres)"
                    : tipoDocumento.Descripcion
            })
            .ToList();
    }

    private static Dictionary<int, int?> BuildCountValidMap(IEnumerable<TipoDocumento> tiposDocumento)
    {
        return tiposDocumento.ToDictionary(tipoDocumento => tipoDocumento.Id, tipoDocumento => tipoDocumento.CountValid);
    }

    private async Task<IdentityResult> SetUserRoleAsync(ApplicationUser user, string role)
    {
        var normalizedRole = AllowedRoles.Contains(role) ? role : "User";
        if (!await roleManager.RoleExistsAsync(normalizedRole))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(normalizedRole));
            if (!roleResult.Succeeded)
            {
                return roleResult;
            }
        }

        var currentRoles = await userManager.GetRolesAsync(user);
        var rolesToRemove = currentRoles.Where(currentRole => currentRole != normalizedRole).ToArray();
        if (rolesToRemove.Length > 0)
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                return removeResult;
            }
        }

        return await userManager.IsInRoleAsync(user, normalizedRole)
            ? IdentityResult.Success
            : await userManager.AddToRoleAsync(user, normalizedRole);
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
