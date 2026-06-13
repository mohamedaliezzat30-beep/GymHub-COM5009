using System.ComponentModel.DataAnnotations;

namespace GymHub.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public string MemberName { get; set; } = string.Empty;

        [Required]
        public int GymClassId { get; set; }

        public GymClass? GymClass { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string BookingStatus { get; set; } = string.Empty;
    }
}