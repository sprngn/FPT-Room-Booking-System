using System;
using System.Collections.Generic;

namespace FPT_Room_Booking_System.Models
{
    public partial class Room
    {
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        public int RoomId { get; set; }
        public int DepartmentId { get; set; }
        public string RoomName { get; set; } = null!;
        public int TypeId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Department Department { get; set; } = null!;
        public virtual RoomType Type { get; set; } = null!;
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
