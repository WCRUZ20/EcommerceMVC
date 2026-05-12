using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[Authorize]
public class OrdersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
