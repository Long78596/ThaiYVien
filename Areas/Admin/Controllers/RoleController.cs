using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Areas.Admin.Dto.Role;
using ThaiYVien.Areas.Admin.Service.IService;
using ThaiYVien.Data;
using ThaiYVien.Models.ViewModel;

namespace ThaiYVien.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class RoleController : Controller
	{

		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IRoleService _roleService;
		private readonly INotyfService _notyfService;
		public RoleController( RoleManager<IdentityRole> roleManager, IRoleService roleService,INotyfService notyfService)
		{
			_roleManager = roleManager;
			_roleService = roleService;
			_notyfService = notyfService;
		}

		public async Task<IActionResult> Index()
		{
			var roles = await _roleService.GetAllRolesAsync();
			var viewModel = new RoleViewModel { Roles =roles };
			return View(viewModel);
        }
		[HttpGet]
		public IActionResult Create()
		{
			return View(new RoleDto());
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		
		public async Task<IActionResult> Create(RoleDto model)
		{
			if (!ModelState.IsValid)
			{
				_notyfService.Error("Quyền đã tồn tại");
				return Redirect(Request.Headers["Referer"]);
			}

			var success = await _roleService.CreateRoleAsync(model);
			if (success)
				_notyfService.Success("Thêm quyền truy cập thành công!");
			else
				_notyfService.Error("Quyền đã tồn tại");

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			var role = await _roleService.GetRoleByIdAsync(id);
			if (role == null)
				return NotFound();

			return View(role);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(string id, RoleDto model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var success = await _roleService.UpdateRoleAsync(id, model);
			if (success)
				_notyfService.Success("Cập nhật quyền thành công!");
			else
				_notyfService.Error("Lỗi khi cập nhật quyền ");

			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			var success = await _roleService.DeleteRoleAsync(id);
			if (success)
				_notyfService.Success("Xóa quyền thành công!");
			else
				_notyfService.Error("Lỗi khi xóa quyền ");


			return RedirectToAction("Index");
		}
	}
}

