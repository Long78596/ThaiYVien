using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;
using ThaiYVien.Migrations;
using ThaiYVien.Models;

namespace ThaiYVien.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryServiceController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly INotyfService _notyfService;
		public CategoryServiceController(ApplicationDbContext context, INotyfService notyfService)
		{
			_context = context;
			_notyfService = notyfService;
		}
		public IActionResult Index()
		{
			var category = _context.CategoryServices.ToList();
			return View(category);
		}
		[HttpGet]
		public IActionResult Create()
		{

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryServiceModel category)
		{

			var category_name = await _context.CategoryServices.FirstOrDefaultAsync(p => p.Name == category.Name);
			if (category_name != null)
			{
				_notyfService.Error("Danh mục đã có trong đã tồn tại!");
				return View(category);
			}
			_context.Add(category);
			await _context.SaveChangesAsync();
			_notyfService.Success("Thêm mới danh mục thành công!");
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int Id)
		{
			var category = await _context.CategoryServices.FindAsync(Id);
			if (category == null)
			{
				return NotFound(); // Return 404 if not found
			}
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CategoryServiceModel categorymodel)
		{

			var exists_category = await _context.CategoryServices.FindAsync(categorymodel.Id);

			var category_name = await _context.CategoryServices.FirstOrDefaultAsync(p => p.Name == categorymodel.Name);

			if (category_name != null && category_name.Id != categorymodel.Id)
			{
				_notyfService.Error("Danh mục  đã tồn tại!");
				return View(category_name);
			}
			if (exists_category == null)
			{
				return NotFound();
			}
			exists_category.Name = categorymodel.Name;

			_context.Update(exists_category);
			await _context.SaveChangesAsync();
			_notyfService.Success("Cập nhật danh mục thành công!");
			return RedirectToAction("Index");

		}


		public async Task<IActionResult> Delete(int Id)
		{
			var category = await _context.CategoryServices.FindAsync(Id);
			if (category == null)
			{
				return NotFound();
			}
			_context.CategoryServices.Remove(category);
			await _context.SaveChangesAsync();
			_notyfService.Success("Xóa danh mục thành công!");
			return RedirectToAction("Index");
		}
	}

}

