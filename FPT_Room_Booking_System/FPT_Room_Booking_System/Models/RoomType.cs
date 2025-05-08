using System;
using System.Collections.Generic;

namespace FPT_Room_Booking_System.Models
{
    public partial class RoomType
    {
        public RoomType()
        {
            Rooms = new HashSet<Room>();
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public int Capacity { get; set; }
        public bool Projector { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
