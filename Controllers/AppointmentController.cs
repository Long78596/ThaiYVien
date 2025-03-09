using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;
using ThaiYVien.Migrations;
using ThaiYVien.Models;
using ThaiYVien.Data.Dto.Appointment;
using System.Security.Claims;
using ThaiYVien.Data.Dto.User;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace ThaiYVien.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyfService;
        private readonly UserManager<AppUserModel> _userManager;
        public AppointmentController(ApplicationDbContext context, INotyfService notyfService, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _notyfService = notyfService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Locations = await _context.Locations.ToListAsync();


            var categories = await _context.CategoryServices.ToListAsync();

            var servicesByCategory = await _context.Services
                .Where(s => s.CategoryServiceId.HasValue) // Bỏ các dịch vụ có CategoryId NULL
                .GroupBy(s => s.CategoryServiceId.Value) // Chuyển CategoryId? => int
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            //return Json(new { categories, servicesByCategory });
            ViewBag.Employees = await _context.Users
                .Where(u => u.RoleId == "27300127-bd78-4ec4-a648-9051ca099ebc")
                .ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.ServicesByCategory = servicesByCategory;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Appointment()
        {


            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "user không tồn tại!";
                return RedirectToAction("Index", "Home");
            }

            var appointment = await _context.Appointments
         .Where(a => a.UserId == userId)
         .OrderByDescending(a => a.SlotDate)
         .ThenByDescending(a => a.Id)
         .Select(a => new AppointmentDto
         {

             SlotDate = a.SlotDate,
             SlotTime = a.SlotTime,
             ServiceName = a.Service.Title,
             Amount = a.Amount,
             UserName = a.User.FullName,
             PhoneNumber = a.User.PhoneNumber,
             Email = a.User.Email,
             Note = a.Notes,
         })
         .FirstOrDefaultAsync();

            return View(appointment);



        }
        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.User == null || string.IsNullOrEmpty(model.User.Email))
            {
                return BadRequest(new { Message = "User data (FullName, Phone, Email) is required" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.User.Email);

            if (user == null)
            {
                user = new AppUserModel
                {
                    Id = Guid.NewGuid().ToString(),
                    FullName = model.User.FullName,
                    Email = model.User.Email,
                    PhoneNumber = model.User.PhoneNumber
                };
                HttpContext.Session.SetString("UserId", user.Id);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {

                user.FullName = model.User.FullName ?? user.FullName;
                user.PhoneNumber = model.User.PhoneNumber ?? user.PhoneNumber;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ID == model.ServiceId);
            if (service == null)
            {
                return BadRequest(new { Message = "Service not found" });
            }
            


            var appointment = new AppointmentModel
            {
                UserId = user.Id,
                LocationId = model.LocationId,
                ServiceId = model.ServiceId,
                SlotDate = model.SlotDate,
                SlotTime = model.SlotTime,
                Amount = service.Price,
                Notes = model.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            _notyfService.Success("Đã đặt lịch thành công!");
            TempData["Success"] = "Đã đặt lịch thành công";
            HttpContext.Session.SetString("UserId", user.Id);
           


            return Ok(new { Message = "Appointment created successfully", AppointmentId = appointment.Id });
        }


        [HttpGet]
        public async Task<IActionResult> HistoryAppointment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDto
                {
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                TempData["error"] = "Không tìm thấy thông tin người dùng!";
                return RedirectToAction("Index", "Home");
            }

            var appointments = await _context.Appointments
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.SlotDate)
                .ThenByDescending(a => a.Id)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    SlotDate = a.SlotDate,
                    SlotTime = a.SlotTime,
                    ServiceName = a.Service.Title,
                    Amount = a.Amount,
                    IsCompleted = a.IsCompleted,
                    Cancelled = a.Cancelled,
                    Payment = a.Payment,
                    Note = a.Notes
                })
                .ToListAsync();

            var model = new HistoryAppoitmentDto
            {
                User = user,
                Appointments = appointments
            };
            return View(model);




        }


        public async Task<IActionResult> CancelAppointment(int Id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var order = await _context.Appointments.FirstOrDefaultAsync(o => o.Id == Id);
                if (order == null)
                {
                    _notyfService.Warning("Không tìm thấy .");
                    return RedirectToAction("HistoryAppointment", "Appointment");
                }


                //var timeSinceOrder = DateTime.Now - order.CreateDate;


                order.Cancelled = true;
                _context.Appointments.Update(order);
                await _context.SaveChangesAsync();

                TempData["Error"] = "hủy hẹn lịch thành công";
                return RedirectToAction("HistoryAppointment", "Appointment");
            }
            catch (Exception ex)
            {


                _notyfService.Error("Đã xảy ra lỗi khi hủy đơn hàng. Vui lòng thử lại sau.");
                return RedirectToAction("HistoryAppointment", "Appointment");
            }

        }

        

    }

}

