namespace CarWashApp.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public DateTime Date { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; }
    }
}
