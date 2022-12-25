namespace CarWashApp.DTOs
{
    public class CarWashDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public uint Size { get; set; } //available car slots
        public string WorkingHours { get; set; }
        public double TotalRating { get; set; }
        public List<ServiceDTO> Services { get; set; }
    }
}
