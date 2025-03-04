using System.ComponentModel.DataAnnotations.Schema;
using ThaiYVien.Repository.File;

namespace ThaiYVien.Models
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
        public virtual ICollection<AppointmentModel>? Appointments { get; set; } = new List<AppointmentModel>();
    }
}
