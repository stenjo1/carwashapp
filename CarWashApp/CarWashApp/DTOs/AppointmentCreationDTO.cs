using CarWashApp.Entities;

namespace CarWashApp.DTOs
{
    public class AppointmentCreationDTO
    {
        public int CarWashId { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public DateTime Date { get; set; }
        public string ServiceType { get; set; }
    }
}
