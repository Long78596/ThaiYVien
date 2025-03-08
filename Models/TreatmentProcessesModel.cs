using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Models
{
    public class TreatmentProcessesModel
    {
        [Key]
        public int Id { get; set; } 
        public int StepNumber { get; set; } 
        public string? Description { get; set; }  

       
        public int? ServiceId { get; set; }
        public ServiceModel Service { get; set; } 
    }
}
