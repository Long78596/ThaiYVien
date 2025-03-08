using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThaiYVien.Data;
using Microsoft.AspNetCore.Authentication.Google;
using ThaiYVien.Models;
using ThaiYVien.Services;
using ThaiYVien.Services.IService;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Repository.Email;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Security.Claims;
using ThaiYVien.Data.Dto.User;
using ThaiYVien.Data.Dto.Appointment;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;
using ThaiYVien.Data.Dto;

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
        private readonly INotyfService _notyfService;
        public AccountController(
           UserManager<AppUserModel> userManager,
           SignInManager<AppUserModel> signInManager,

              ApplicationDbContext db,
            IAccountService accountService,
             IUserService userService,
            IEmailSender emailSender,
            ApplicationDbContext context,
            INotyfService notyfService
             
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _accountService = accountService;
            _userService = userService;
            _emailSender = emailSender;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> LoginByGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", "Account")  
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }



        public async Task<IActionResult> GoogleResponse()
        {
            var error = HttpContext.Request.Query["error"];

            if (!string.IsNullOrEmpty(error) && error == "access_denied")
            {
                _notyfService.Error("Bạn đã hủy đăng nhập");
                return RedirectToAction("Login", "Account");
            }
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value

            });
            //_notyfService.Success("Đăng nhập thành công");
            //return RedirectToAction("Index", "Home");
            //return Json(claims);
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string UserName = email.Split("@")[0];
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var passwordHasher = new PasswordHasher<AppUserModel>();
                var hashedPassword = passwordHasher.HashPassword(null, "123456qa");
                var newUser = new AppUserModel { UserName = UserName, Email = email };
                newUser.PasswordHash = hashedPassword;
                var createUserResult = await _userManager.CreateAsync(newUser);
                if (!createUserResult.Succeeded)
                {
                    _notyfService.Error("Đăng nhập thất bại");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    _notyfService.Success("Đăng ký thành công");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                await _signInManager.SignInAsync(existingUser, isPersistent: false);
                _notyfService.Success("Đăng nhập thành công");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
        }
     
        public async Task LoginByFacebook()
        {
            await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("FacebookResponse")

                });
        }
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                _notyfService.Error("Không thể lấy email từ Facebook");
                return RedirectToAction("Login", "Account");
            }

            string UserName = email.Split('@')[0];
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
            {
                var passwordHasher = new PasswordHasher<AppUserModel>();
                var hashedPassword = passwordHasher.HashPassword(null, "123456qa");
                var newUser = new AppUserModel { UserName = UserName, Email = email };
                newUser.PasswordHash = hashedPassword;
                var createUserResult = await _userManager.CreateAsync(newUser);
                if (!createUserResult.Succeeded)
                {
                    _notyfService.Error("Đăng nhập thất bại");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    _notyfService.Success("Đăng ký thành công");
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                await _signInManager.SignInAsync(existingUser, isPersistent: false);
                _notyfService.Success("Đăng nhập thành công");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");


        }
        [HttpGet]
        public ActionResult Create()
        {
            _notyfService.Success("Chào mừng đến Viện dưỡng Sinh và chăm sóc sức khỏe Trung tâm thái Y Viện !");

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
            

            _notyfService.Success("Chào mừng đến Viện dưỡng Sinh và chăm sóc sức khỏe Trung tâm thái Y Viện !");

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
                    TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                    return PartialView(loginDto);
                }
            }
            return PartialView(loginDto);
        }

        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();
            TempData["error"] = "Đăng xuất thành công";
            _notyfService.Error("Đã đăng xuất thành công");
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
                _notyfService.Error("Định dạng email không hợp lệ!");
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

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            var model = new UpdateUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Gender = user.Gender
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> UpdateInfoAccount(UpdateUserDto user)
        {
            if (!ModelState.IsValid)
            {


                TempData["error"] = "Dữ liệu không hợp lệ!";
                return RedirectToAction("Index");
            }



            var userById = _db.Users.Find(user.Id);
            if (userById == null)
            {
                return NotFound();
            }




            if (!string.IsNullOrEmpty(user.NewPasssword))
            {
                if (user.NewPasssword.Length < 6 || !user.NewPasssword.Any(char.IsLetter) || !user.NewPasssword.Any(char.IsDigit))
                {
                    TempData["Error"] = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm cả chữ và số!";
                    return RedirectToAction("Index");
                }

                var passwordHasher = new PasswordHasher<AppUserModel>();
                userById.PasswordHash = passwordHasher.HashPassword(userById, user.NewPasssword);
            }


            userById.Email = user.Email;
            userById.UserName = user.Username;
            userById.PhoneNumber = user.PhoneNumber;
            userById.FullName = user.FullName;
            userById.Gender = user.Gender;
            userById.Address = user.Address;

            _db.Update(userById);
            await _db.SaveChangesAsync();

            TempData["success"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangePassword", model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            var passwordHasher = new PasswordHasher<AppUserModel>();

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("CurrentPassword", "Mật khẩu cũ không đúng!");
                return View("ChangePassword", model);
            }

            var passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");
            if (!passwordRegex.IsMatch(model.NewPassword))
            {
                ModelState.AddModelError("NewPassword", "Mật khẩu mới phải có ít nhất 6 ký tự, bao gồm cả chữ và số.");
                return View("ChangePassword", model);
            }

            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError("ConfirmNewPassword", "Mật khẩu xác nhận không khớp!");
                TempData["error"] = "mật khẩu xác nhận không khớp!";
                return View("ChangePassword", model);
            }




            user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
            _db.Users.Update(user);
            _db.SaveChanges();

            TempData["success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("ChangePassword");
        }







    }






}

