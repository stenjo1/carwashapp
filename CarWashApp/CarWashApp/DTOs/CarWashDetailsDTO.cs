namespace CarWashApp.DTOs
{
    public class CarWashDetailsDTO : CarWashDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<AppointmentCarWashDTO> Appointments { get; set; }
    }
}
