using Microsoft.AspNetCore.Mvc;

namespace CarWashApp.DTOs
{
    public class RevenueFilterCreationDTO
    {
        [FromRoute]
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
