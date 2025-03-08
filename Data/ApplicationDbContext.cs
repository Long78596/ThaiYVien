using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThaiYVien.Models;

namespace ThaiYVien.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<CategoryServiceModel> CategoryServices { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<TreatmentProcessesModel> TreatmentProcesses { get; set; }
        public DbSet<LocationModel> Locations { get; set; }
        public DbSet<AppointmentModel> Appointments { get; set; }

    }
}
