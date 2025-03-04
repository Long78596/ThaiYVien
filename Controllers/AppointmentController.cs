using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;
using ThaiYVien.Migrations;
using ThaiYVien.Models;

namespace ThaiYVien.Controllers
{
	public class AppointmentController : Controller
	{
		private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyfService;
		public AppointmentController(ApplicationDbContext context, INotyfService notyfService)
		{
			_context = context;
            _notyfService = notyfService;
		}
		public  async Task<IActionResult> Index()
		{
			ViewBag.Locations = await _context.Locations.ToListAsync();

			ViewBag.Services1 = await _context.Services.Where(cb=>cb.Status == 0).ToListAsync();
			ViewBag.Services2 = await _context.Services.Where(cb=>cb.Status == 1).ToListAsync();
			ViewBag.Services3 = await _context.Services.Where(cb=>cb.Status == 2).ToListAsync();
            ViewBag.Employees = await _context.Users
				.Where(u => u.RoleId == "27300127-bd78-4ec4-a648-9051ca099ebc") 
				.ToListAsync();
         
            return View();
		}
        [HttpGet]
         public async Task<IActionResult> Appointment()
        {
            return View();
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
            

            
            return Ok(new { Message = "Appointment created successfully", AppointmentId = appointment.Id });
        }


    }
}
