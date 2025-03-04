using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;
using ThaiYVien.Models;

namespace ThaiYVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocationController : Controller
    {
        ApplicationDbContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnviorment;
        public LocationController(ApplicationDbContext dataContext, IWebHostEnvironment webHostEnviorment)
        {
            _dataContext = dataContext;
            _webHostEnviorment = webHostEnviorment;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _dataContext.Locations.OrderByDescending(p => p.Id).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View(new LocationModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationModel location)
        {


            var check_name = await _dataContext.Locations.FirstOrDefaultAsync(p => p.Name == location.Name);
            if (check_name != null)
            {
                TempData["error"] = "Dịch vụ đã tồn tại!";
                return View();
            }

            if (location.ImageUpload != null)
            {
                string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/location");

                string imagename = Guid.NewGuid().ToString() + "_" + location.ImageUpload.FileName;
                string filePath = Path.Combine(uploadDir, imagename);

                FileStream fs = new FileStream(filePath, FileMode.Create);
                await location.ImageUpload.CopyToAsync(fs);
                fs.Close();
                location.ImageUrl = imagename;

            }



            _dataContext.Add(location);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thêm dịch vụ thành công!";
            return RedirectToAction("Index");


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            LocationModel location = await _dataContext.Locations.FindAsync(Id);
            return View(location);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LocationModel location)
        {


            if (location.Id == null)
            {
                TempData["error"] = "mã dịch vụ không tồn tại";
                return View(location);
            }

            var exists_location = await _dataContext.Locations.FindAsync(location.Id);


            if (exists_location == null)
            {
                TempData["error"] = "tên dịch vụ không tồn tại"; ;
                return View(location);
            }



            if (location.ImageUpload != null)
            {
                string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/location");
                string imageName = Guid.NewGuid().ToString() + "_" + location.ImageUpload.FileName;
                string filePath = Path.Combine(uploadDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await location.ImageUpload.CopyToAsync(fs);
                }
                exists_location.ImageUrl = imageName;
            }

            exists_location.Address = location.Address;
            exists_location.Name = location.Name;
         

            _dataContext.Update(exists_location);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Cập nhật thành công";
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int Id)
        {
            LocationModel locations = await _dataContext.Locations.FindAsync(Id);
            if (!string.Equals(locations.ImageUrl, "noname.jpg"))
            {
                string uploadDir = Path.Combine(_webHostEnviorment.WebRootPath, "image/location");
                string filePath = Path.Combine(uploadDir, locations.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _dataContext.Locations.Remove(locations);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Xóa thành công";
            return RedirectToAction("Index");
        }


    }
}
