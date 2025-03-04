using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;
using ThaiYVien.Migrations;
using ThaiYVien.Areas.Admin.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThaiYVien.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using ThaiYVien.Areas.Admin.Dto.User;
using ThaiYVien.Areas.Admin.Service.IService;
using Microsoft.Identity.Client;

namespace ThaiYVien.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/User")]
	public class UserController : Controller
	{

		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUserModel> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<AppUserModel> _signInManager;
		private readonly INotyfService _notyf;
		private readonly IUserAdminService _adminService;
		public UserController(ApplicationDbContext context, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUserModel> signInManager, INotyfService notyf, IUserAdminService userAdminService)

		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_notyf = notyf;
			_adminService = userAdminService;
		}

		[HttpGet]
		[Route("Index")]
		public async Task<IActionResult> Index()
		{
			var usersWithRoles = await _adminService.GetAllUsersAsync();
			return View(usersWithRoles);
		}
		[HttpGet]
		[Route("Detail/{id}")]
		public async Task<IActionResult> Detail(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				_notyf.Error("không tồn tại!");
				return NotFound();
			}

			var userWithRole = await _adminService.GetUserByIdAsync(id);

			if (userWithRole == null)
			{
				_notyf.Error("User không tồn tại");
				return NotFound();
			}

			return View(userWithRole);
		}


		[HttpGet]
		[Route("Create")]
		public async Task<IActionResult> Create()
		{
			 
			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles, "Id", "Name");
			return View(new UserCreateDto());
		}
		
		
		[HttpPost]
		[Route("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserCreateDto userDto)
		{
			var message = await _adminService.CreateUserAsync(userDto);

			if (message == "Thêm người dùng thành công!")
			{
				_notyf.Success(message);
				return RedirectToAction("Index", "User");
			}

			_notyf.Error(message);
			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles, "Id", "Name");
			return View(userDto);
		}

		private void AddIdentityErrors(IdentityResult identityResult)
		{
			foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}
		[HttpGet]
		[Route("Delete/{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var success = await _adminService.DeleteUserAsync(id);
			if (!success)
			{
				_notyf.Error("Lỗi khi xóa người dùng!");
			}
			_notyf.Success("Đã Xóa thành công!");
			return RedirectToAction("Index");
		}
		[HttpGet]
		[Route("Edit")]
		public async Task<IActionResult> Edit(string Id)
		{
			if (string.IsNullOrEmpty(Id))
			{
				return NotFound();

			}
			var user = await _userManager.FindByIdAsync(Id);
			if (user == null)
			{
				return NotFound();
			}
			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles, "Id", "Name");
			return View(user);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Update")]
		public async Task<IActionResult> Update(string Id, AppUserModel model)
		{
			var existing = await _userManager.FindByIdAsync(Id);
			if (existing == null)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				
				existing.UserName = model.UserName;
				existing.Email = model.Email;
				existing.PhoneNumber = model.PhoneNumber;
				existing.Gender = model.Gender;
				existing.Address = model.Address;
				existing.RoleId = model.RoleId;
				existing.FullName = model.FullName;

				var role = await _roleManager.FindByIdAsync(model.RoleId);
				if (role == null)
				{
					ModelState.AddModelError("", "Role không tồn tại.");
					return View(existing);
				}

				var currentRoles = await _userManager.GetRolesAsync(existing);
				await _userManager.RemoveFromRolesAsync(existing, currentRoles);

				var addRoleResult = await _userManager.AddToRoleAsync(existing, role.Name);
				if (!addRoleResult.Succeeded)
				{
					AddIdentityErrors(addRoleResult);
					return View(existing);
				}

				var updateUserResult = await _userManager.UpdateAsync(existing);
				if (updateUserResult.Succeeded)
				{
					TempData["success"] = "Cập nhật người dùng thành công!";
					return RedirectToAction("Index", "User");
				}
				else
				{
					AddIdentityErrors(updateUserResult);
					return View(existing);
				}
			}

			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles, "Id", "Name");
			var errors = ModelState.Values.SelectMany(v => v.Errors.Select(x => x.ErrorMessage)).ToList();
			string errorMessage = string.Join("\n", errors);
			return View(existing);
		}
		


	}
}
