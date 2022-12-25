
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace CarWashApp.Entities
{
    public class Customer : ApplicationUser
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Point Location { get; set; }
        public double Wallet { get; set; } = 0d;
        public List<Appointment> Appointments { get; set; }
    }
}
