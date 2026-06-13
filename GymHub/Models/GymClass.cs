using System;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Models
{
    public class GymClass
    {
        public int GymClassId { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        public DateTime ClassDate { get; set; }

        [Required]
        public string StartTime { get; set; } = string.Empty;

        [Required]
        public string EndTime { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Instructor { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string DifficultyLevel { get; set; } = string.Empty;

        [Required]
        [Range(1, 50)]
        public int Capacity { get; set; }
    }
}