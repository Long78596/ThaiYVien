using Microsoft.AspNetCore.Mvc;

namespace ThaiYVien.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
