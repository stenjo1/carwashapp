using Microsoft.AspNetCore.Mvc;

namespace CarWashApp.DTOs
{
    public class ServiceCreationDTO
    {
        [FromRoute]
        public int Id { get; set; }
        public string ServiceType { get; set; }
        public double Price { get; set; }
        public int Duration { get; set; } = 1;
    }
}
