using System;
using System.Collections.Generic;

namespace FPT_Room_Booking_System.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public int Role { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsActive { get; set; }
        public string? EmailKey { get; set; }
        public string Password { get; set; } = null!;

        public virtual Role RoleNavigation { get; set; } = null!;
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
