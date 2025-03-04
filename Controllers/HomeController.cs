using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThaiYVien.Models;

namespace ThaiYVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyfService;
        public HomeController(ILogger<HomeController> logger, INotyfService notyfService)
        {
            _logger = logger;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            _notyfService.Success("Chào mừng!");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
