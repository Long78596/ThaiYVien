using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;

namespace ThaiYVien.Repository.Components
{
    public class CategoryServiceViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _dataContext;
        public CategoryServiceViewComponent(ApplicationDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var danhMucs = await _dataContext.CategoryServices.ToListAsync();


            return View(danhMucs);
        }
    }
}
