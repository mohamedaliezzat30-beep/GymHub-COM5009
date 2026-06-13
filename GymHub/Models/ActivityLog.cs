using System;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Models
{
    public class ActivityLog
    {
        public int ActivityLogId { get; set; }

        public string? UserEmail { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}