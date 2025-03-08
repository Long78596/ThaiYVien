using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;

namespace ThaiYVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryServiceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var category = _context.CategoryServices.ToList();
            return View(category);
        }

    }
}
