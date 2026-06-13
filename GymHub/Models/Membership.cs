using System;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Models
{
    public class Membership
    {
        public int MembershipId { get; set; }

        [Required]
        [StringLength(100)]
        public string MemberName { get; set; } = string.Empty;

        public string? UserEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string MembershipType { get; set; } = string.Empty;

        [Required]
        [Range(1, 500)]
        public decimal Price { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;
    }
}