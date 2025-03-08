using System.ComponentModel.DataAnnotations;

namespace ThaiYVien.Models
{
    public class CategoryServiceModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
