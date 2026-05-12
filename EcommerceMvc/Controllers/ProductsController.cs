using Microsoft.AspNetCore.Mvc;

namespace EcommerceMvc.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
