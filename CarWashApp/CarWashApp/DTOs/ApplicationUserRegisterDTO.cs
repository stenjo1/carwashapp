using CarWashApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace CarWashApp.DTOs
{
    public class ApplicationUserRegisterDTO
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{3,15}")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [MinLength(3)]
        public string FirstName { get; set; }
        [MinLength(3)]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }
        public bool IsOwner { get; set; }

        //properties will be hidden if user checked owner
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
