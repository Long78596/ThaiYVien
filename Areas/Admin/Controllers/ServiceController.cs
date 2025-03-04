using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;
using ThaiYVien.Models;

namespace ThaiYVien.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ServiceController : Controller
    {   ApplicationDbContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnviorment;
        public ServiceController(ApplicationDbContext dataContext, IWebHostEnvironment webHostEnviorment)
        {
            _dataContext = dataContext;
            _webHostEnviorment = webHostEnviorment;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _dataContext.Services.OrderByDescending(p => p.ID).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceModel service)
        {

            
            var check_name = await _dataContext.Services.FirstOrDefaultAsync(p => p.Title == service.Title);
            if (check_name != null)
            {
                TempData["error"] = "Dịch vụ đã tồn tại!";
                return View();
            }

            if (service.ImageUpload != null)
            {
                string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/service");

                string imagename = Guid.NewGuid().ToString() + "_" + service.ImageUpload.FileName;
                string filePath = Path.Combine(uploadDir, imagename);

                FileStream fs = new FileStream(filePath, FileMode.Create);
                await service.ImageUpload.CopyToAsync(fs);
                fs.Close();
                service.ImageUrl = imagename;

            }
            


            _dataContext.Add(service);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thêm dịch vụ thành công!";
            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ServiceModel service = await _dataContext.Services.FindAsync(Id);
            return View(service);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServiceModel service)
        {


            if (service.ID == null)
            {
                TempData["error"] = "mã dịch vụ không tồn tại";
                return View(service);
            }

            var exists_service = await _dataContext.Services.FindAsync(service.ID);


            if (exists_service == null)
            {
                TempData["error"] = "tên dịch vụ không tồn tại"; ;
                return View(service);
            }

         

            if (service.ImageUpload != null)
            {
                string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/service");
                string imageName = Guid.NewGuid().ToString() + "_" + service.ImageUpload.FileName;
                string filePath = Path.Combine(uploadDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await service.ImageUpload.CopyToAsync(fs);
                }
                exists_service.ImageUrl = imageName;
            }

            exists_service.Title = service.Title;
            exists_service.Description = service.Description;
            exists_service.Duration = service.Duration;
            exists_service.Price =service.Price;
            //exists_food.Video = monan.Video;
            exists_service.TreatmentDate =service.TreatmentDate;
            exists_service.Status = service.Status;

            _dataContext.Update(exists_service);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Cập nhật thành công";
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int Id)
        {
           ServiceModel service = await _dataContext.Services.FindAsync(Id);
            if (!string.Equals(service.ImageUrl, "noname.jpg"))
            {
                string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/service");
                string filePath = Path.Combine(uploadDir, service.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _dataContext.Services.Remove(service);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa thành công";
            return RedirectToAction("Index");
        }


    }
}
