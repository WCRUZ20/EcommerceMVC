using Ecommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers;

[Authorize]
public class ProductsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = await context.Products
            .AsNoTracking()
            .OrderBy(product => product.ShortDescripcion)
            .ThenBy(product => product.Sku)
            .ToListAsync();

        return View(products);
    }

    public IActionResult DiscountDefinition()
    {
        return View();
    }
}
