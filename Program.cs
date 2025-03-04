using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Data;
using ThaiYVien.Models;
using ThaiYVien.Repository.IRepository;
using ThaiYVien.Repository;
using ThaiYVien.Services.IService;
using ThaiYVien.Services;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.Google;
using ThaiYVien.Repository.Email;
using ThaiYVien.Areas.Admin.Repository.IRepository;
using ThaiYVien.Areas.Admin.Repository;
using ThaiYVien.Areas.Admin.Service.IService;
using ThaiYVien.Areas.Admin.Service;
using ThaiYVien.Areas.Admin.Mappings;

namespace ThaiYVien
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var builderRazer = builder.Services.AddRazorPages();
			builder.Services.AddNotyf(config =>
			{
				config.DurationInSeconds = 5;
				config.IsDismissable = true;
				config.Position = NotyfPosition.TopRight;
			}
);

			builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDB")));
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<IUserAdminRepository, UserAdminRepository>();
            builder.Services.AddScoped<IUserAdminService,UserAdminService>();
			builder.Services.AddScoped<IRoleRepository, RoleRepository>();
			builder.Services.AddScoped<IRoleService, RoleService>();


			


			builder.Services.AddIdentity<AppUserModel, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                // options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.AllowedForNewUsers = true;

                // User settings.
                //options.User.AllowedUserNameCharacters =
                //"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                //options.User.RequireUniqueEmail = false;
            });
			builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

			builder.Services.AddAuthentication(options =>
            {
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
                options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;

            });
			builder.Services.AddAuthentication().AddFacebook(opt =>
			{
				opt.ClientId = "";
				opt.ClientSecret = "";
			});

			var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseNotyf();
            app.MapControllerRoute(
           name: "Areas",
          pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
           

            app.Run();
        }
    }
}
