using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}