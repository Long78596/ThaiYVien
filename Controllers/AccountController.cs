using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThaiYVien.Data;
using Microsoft.AspNetCore.Authentication.Google;
using ThaiYVien.Data.Dto;
using ThaiYVien.Models;
using ThaiYVien.Services;
using ThaiYVien.Services.IService;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Repository.Email;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace ThaiYVien.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        public AccountController(
           UserManager<AppUserModel> userManager,
           SignInManager<AppUserModel> signInManager,
           
              ApplicationDbContext db,
            IAccountService accountService,
             IUserService userService,
            IEmailSender emailSender
             
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _accountService = accountService;
            _userService = userService;
            _emailSender = emailSender;
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            return PartialView(new RegisterDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<ActionResult> Create(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            var result = await _userService.RegisterUserAsync(model);

            if (result.Success)
            {

                TempData["Success"]= result.Message;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["Error"] = result.Message;
                return PartialView(model);
            }
        }
        
        [HttpGet]
        public ActionResult Login()
        {
           
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loginResult = await _accountService.LoginAsync(loginDto);
                    if (loginResult.Success)
                    {
                        TempData["Success"] = loginResult.Message;

                        
                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        TempData["Error"] = loginResult.Message;
                        return PartialView(loginDto);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Có lỗi xảy ra";

                }
            }
            return PartialView(loginDto);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();
            TempData["Error"] = "Đăng xuất thành công!!";
            return RedirectToAction("Login", "Account");
        }


        public IActionResult ForgetPass()
        {
            return PartialView();
        }
        public IActionResult NewPass()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMailForgetPass(AppUserModel user)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var regex = new Regex(emailPattern);

            
            if (string.IsNullOrEmpty(user.Email) || !regex.IsMatch(user.Email))
            {
                TempData["Error"] = "Định dạng email không đúng, Xin vui lòng nhập lại!";

                return Redirect(Request.Headers["Referer"]);
            }
            var checkMail = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (checkMail == null)
            {
                TempData["Error"] = "Email ko tồn tại hoặc không đúng!";
               
                return RedirectToAction("ForgetPass", "Account");
            }
            else
            {
                string token = Guid.NewGuid().ToString();

                checkMail.Token = token;
                _db.Update(checkMail);
                await _db.SaveChangesAsync();
                var recevier = checkMail.Email;
                var subject = "Thay đổi mật khẩu tài khoản  " + checkMail.Email;
                var message = "Hãy nhấn vào đường link để thay đổi mật khẩu" + " <a href='" + $"{Request.Scheme}://{Request.Host}/Account/NewPass?email=" + checkMail.Email + "&token=" + token + " ' ></a>";

                await _emailSender.SendEmailAsync(recevier, subject, message);
                TempData["Success"] = "Vui lòng kiểm tra email của bạn!";

            }
           
            return RedirectToAction("ForgetPass", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNewPass(AppUserModel user, string token)
        {
            if (string.IsNullOrWhiteSpace(user.PasswordHash) || user.PasswordHash.Length < 6 || !user.PasswordHash.Any(char.IsLetter) || !user.PasswordHash.Any(char.IsDigit))
            {
                TempData["Error"] = "Mật khẩu phải đủ 6 ký tự bao gồm cả chữ và số!";

                return Redirect(Request.Headers["Referer"]);
            }
            var checkuser = await _userManager.Users.Where(u => u.Token == user.Token).FirstOrDefaultAsync();
            if (checkuser != null)
            {
                string newtoken = Guid.NewGuid().ToString();

                var passwordHasher = new PasswordHasher<AppUserModel>();
                var passwordHash = passwordHasher.HashPassword(checkuser, user.PasswordHash);
                checkuser.PasswordHash = passwordHash;

                checkuser.Token = newtoken;

                await _userManager.UpdateAsync(checkuser);
                TempData["Success"] = "Cập nhật mật khẩu thành công!";

                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["Error"] = "Cập nhật mật khẩu thất bại!";

                return RedirectToAction("ForgetPass", "Account");
            }
        }
        public async Task LoginByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")

                });
        }
		public async Task LoginByFacebook()
		{
			await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme,
				new AuthenticationProperties
				{
					RedirectUri = Url.Action("FacebookResponse")

				});
		}


	}






}

