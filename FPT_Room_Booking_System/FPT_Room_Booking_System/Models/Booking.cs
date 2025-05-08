using System;
using System.Collections.Generic;

namespace FPT_Room_Booking_System.Models
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int BookingUser { get; set; }
        public DateTime BookingDate { get; set; }
        public int Slot { get; set; }
        public string Status { get; set; } = null!;
        public bool? IsActive { get; set; }

        public virtual User BookingUserNavigation { get; set; } = null!;
        public virtual Room Room { get; set; } = null!;
        public virtual Slot SlotNavigation { get; set; } = null!;
    }
}
