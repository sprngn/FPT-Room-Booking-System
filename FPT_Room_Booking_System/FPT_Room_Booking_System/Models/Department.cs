using System;
using System.Collections.Generic;

namespace FPT_Room_Booking_System.Models
{
    public partial class Department
    {
        public Department()
        {
            Rooms = new HashSet<Room>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public bool? IsActive { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
