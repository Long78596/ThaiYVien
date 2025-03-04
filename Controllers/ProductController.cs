using Microsoft.AspNetCore.Mvc;

namespace ThaiYVien.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
