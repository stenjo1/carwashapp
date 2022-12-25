using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarWashApp.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }
    }
}
