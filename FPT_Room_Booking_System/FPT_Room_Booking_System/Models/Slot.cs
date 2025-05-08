using System;
using System.Collections.Generic;

namespace FPT_Room_Booking_System.Models
{
    public partial class Slot
    {
        public Slot()
        {
            Bookings = new HashSet<Booking>();
        }
        public string DisplayValue => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";

        public int SlotId { get; set; }
        public int SlotName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
