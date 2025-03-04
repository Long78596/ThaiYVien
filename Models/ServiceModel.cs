using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThaiYVien.Repository.File;

namespace ThaiYVien.Models
{
    public class ServiceModel
    {

        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; } 
        public string ImageUrl { get; set; }
        public DateTime TreatmentDate { get; set; } = DateTime.Now;
        public byte Status { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
		public virtual ICollection<AppointmentModel>? Appointments { get; set; } = new List<AppointmentModel>();
	}
}
