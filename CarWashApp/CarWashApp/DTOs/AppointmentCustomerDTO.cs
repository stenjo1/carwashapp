using CarWashApp.Entities;

namespace CarWashApp.DTOs
{
    public class AppointmentCustomerDTO : AppointmentDTO
    {
        public string CarWashName { get; set; }
        public bool IsFinished { get; set; }
        public uint Rating { get; set; } = 0u;
    }
}
