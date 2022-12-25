using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace CarWashApp.Entities
{
    public class CarWash
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [Range(1,10)]
        public uint Size { get; set; } = 1u; //available car slots
        [Range(0, 24)]
        public uint OpeningHour { get; set; } = 9u;
        [Range(0, 24)]
        public uint ClosingHour { get; set; } = 17u;
        public int Rating { get; set; } = 0; 
        public int Votes { get; set; } = 0;
        [Range(-90,90)]
        public double Latitude { get; set; }
        [Range(-180,180)]
        public double Longitude { get; set; }
        public Point Location { get; set; }
        public Owner Owner { get; set; }
        public Revenue Revenue { get; set; }
        public List<Service> Services { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
