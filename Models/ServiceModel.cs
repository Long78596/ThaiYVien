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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Duration { get; set; } 
        public string ImageUrl { get; set; }
        public DateTime TreatmentDate { get; set; } = DateTime.Now;
        public byte Status { get; set; }


        //public byte State { get; set; }

 
        public int? CategoryServiceId { get; set; }
        public string? SpecialOffer { get; set; }


        public CategoryServiceModel CategoryService { get; set; }


        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
		public virtual ICollection<AppointmentModel>? Appointments { get; set; } = new List<AppointmentModel>();
		public List<TreatmentProcessesModel> TreatmentProcesses { get; set; } = new List<TreatmentProcessesModel>();
	}
}
