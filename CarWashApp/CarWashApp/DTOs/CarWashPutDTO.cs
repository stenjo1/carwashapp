using Microsoft.AspNetCore.Mvc;

namespace CarWashApp.DTOs
{
    public class CarWashPutDTO : CarWashCreationDTO
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
