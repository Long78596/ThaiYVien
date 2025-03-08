using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;
using ThaiYVien.Models.ViewModel;

namespace ThaiYVien.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext dataContext)
        {
            _context = dataContext;
        }
       


            public async Task<IActionResult> ServiceByCategoryServiceId(int id)
            {
                
                var category = _context.CategoryServices.FirstOrDefault(c => c.Id == id);
                if (category == null) return RedirectToAction("Index");

               
                var servicesByCategory = await _context.Services
                    .Include(s => s.CategoryService) 
                    .Where(s => s.CategoryServiceId == category.Id)
                    .OrderByDescending(s => s.ID)
                    .ToListAsync(); 

                return View(servicesByCategory);
            }

        


        public async  Task<IActionResult> Index() {

            var service = await  _context.Services.Include(s => s.CategoryService).OrderBy(r=>r.Price).ToListAsync();
            return View(service);
        }
        public async Task<IActionResult> Search(string searchtern)
        {
            if (string.IsNullOrWhiteSpace(searchtern))
            {
                TempData["error"]="Vui lòng nhập từ khóa tìm kiếm! ";
                return View();
            }

            var services = await _context.Services.Include(c => c.CategoryService).Where(
                p => p.Title.Contains(searchtern) || p.Description.Contains(searchtern) || p.Price.ToString().Contains(searchtern)

                ||
                p.CategoryService.Name.Contains(searchtern)



            ).ToListAsync();
            ViewBag.Keyword = searchtern;
            return View(services);
        }
		public IActionResult Detail(int Id)
		{
			var service = _context.Services
			.Where(s => s.ID == Id)
			.Select(s => new ServiceDetailViewModel
			{
				ID = s.ID,
				Name = s.Title,
				Duration = s.Duration,
				Price = s.Price,
				Description = s.Description,
                ImgUrl=s.ImageUrl,
				TreatmentProcesses = _context.TreatmentProcesses
									.Where(tp => tp.Id == Id)
									.ToList()
			})
			.FirstOrDefault();

			if (service == null)
			{
				return NotFound();
			}

			return View(service);
		}


	}
}
