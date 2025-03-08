using ThaiYVien.Data.Dto.User;

namespace ThaiYVien.Data.Dto.Appointment
{
    public class HistoryAppoitmentDto
    {
        public UserDto User { get; set; }
        public List<AppointmentDto> Appointments { get; set; }
    }
}
