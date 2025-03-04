using Microsoft.AspNetCore.Identity;

namespace ThaiYVien.Models
{
    public class AppUserModel:IdentityUser
    {
        public string? RoleId { get; set; }
        public string? Token { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
		public string? FullName { get; set; }
		public virtual ICollection<AppointmentModel>? Appointments { get; set; } = new List<AppointmentModel>();
	}
}
