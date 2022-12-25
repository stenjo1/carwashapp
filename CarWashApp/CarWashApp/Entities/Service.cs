using System.ComponentModel.DataAnnotations;

namespace CarWashApp.Entities
{
    public class Service
    {
        public int CarWashId { get; set; }
        public string ServiceType { get; set; }
        public double Price { get; set; }
        [Range(1, 12)]
        public int Duration { get; set; } = 1; //duration in hours
    }

    //public enum ServiceType
    //{
    //    Regular,
    //    Extended,
    //    Premium
    //}
}
