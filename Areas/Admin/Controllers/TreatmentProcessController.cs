using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using ThaiYVien.Data;
using ThaiYVien.Models;

namespace ThaiYVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TreatmentProcessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreatmentProcessController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var processes = await _context.TreatmentProcesses.ToListAsync();
            return View(processes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var process = await _context.TreatmentProcesses.Include(tp => tp.Service)
                .FirstOrDefaultAsync(tp => tp.Id == id);
            if (process == null) return NotFound();
            return View(process);
        }

       
        public IActionResult Create()
        {
            ViewBag.Services = _context.Services.ToList();
            return View();
        }

        // Xử lý thêm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TreatmentProcessesModel model)
        {
            
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
           
           
        }

        // Hiển thị form chỉnh sửa
        public async Task<IActionResult> Edit(int id)
        {
            var process = await _context.TreatmentProcesses.FindAsync(id);
            if (process == null) return NotFound();
            ViewBag.Services = _context.Services.ToList();
            return View(process);
        }

        // Xử lý chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TreatmentProcessesModel model)
        {
            if (id != model.Id) return NotFound();


                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            
        }

        public async Task<IActionResult> Delete(int id)
        {
            var process = await _context.TreatmentProcesses.Include(tp => tp.Service)
                .FirstOrDefaultAsync(tp => tp.Id == id);
            if (process == null) return NotFound();
            return View(process);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var process = await _context.TreatmentProcesses.FindAsync(id);
            if (process != null)
            {
                _context.TreatmentProcesses.Remove(process);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
