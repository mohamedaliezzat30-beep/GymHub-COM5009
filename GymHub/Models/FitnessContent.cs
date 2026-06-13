using System.ComponentModel.DataAnnotations;

namespace GymHub.Models
{
    public class FitnessContent
    {
        public int FitnessContentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string? ImagePath { get; set; }
    }
}