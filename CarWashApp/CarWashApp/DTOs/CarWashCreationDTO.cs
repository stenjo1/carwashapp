namespace CarWashApp.DTOs
{
    public class CarWashCreationDTO
    {
        public string Name { get; set; }
        public uint Size { get; set; } = 1u; //available car slots
        public uint OpeningHour { get; set; } = 9u;
        public uint ClosingHour { get; set; } = 17u;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
