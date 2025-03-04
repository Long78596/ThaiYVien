using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Models
{
    public class AppointmentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        

        
        public int? ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual ServiceModel? Service { get; set; }

     
        public int? LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual LocationModel? Location { get; set; }
        
        public string? UserId { get; set; }

        
        public virtual AppUserModel? User { get; set; }

        [Required]
        public DateTime SlotDate { get; set; }

        [Required]
        public TimeSpan SlotTime { get; set; }

        public decimal? Amount { get; set; }

        public bool Cancelled { get; set; } = false;

        public bool Payment { get; set; } = false;

        public bool IsCompleted { get; set; } = false;

        public string? Notes { get; set; }
    }
}
