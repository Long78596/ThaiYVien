using System.ComponentModel;

namespace ThaiYVien.Data.Dto.Appointment
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan SlotTime { get; set; }
        public string ServiceName { get; set; }
        public decimal? Amount { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Note { get; set; }
        public bool Cancelled { get; set; } = false;

        public bool Payment { get; set; } = false;

        public bool IsCompleted { get; set; } = false;
    }
}
