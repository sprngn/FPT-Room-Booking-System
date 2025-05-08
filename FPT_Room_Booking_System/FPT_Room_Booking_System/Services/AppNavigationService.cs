using FPT_Room_Booking_System.Views;
using FPT_Room_Booking_System.Views.ManagerWindow;
using FPT_Room_Booking_System.Views.UserWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FPT_Room_Booking_System.Services
{
    public class AppNavigationService
    {
        public void OpenRoomWindow(int departmentId, int userId) 
        { 
            ListRoomWindow roomWindow = new ListRoomWindow(departmentId, userId);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            roomWindow.Show();
        }

        public void OpenDepartmentWindow(int userId)
        {
            DepartmentWindow departmentWindow = new DepartmentWindow(userId);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            departmentWindow.Show();
        }        
        public void OpenMyBookingWindow(int userId)
        {
            MyBookingWindow myBookingWindow = new MyBookingWindow(userId);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            myBookingWindow.Show();
        }

        public void OpenRequestListWindow(int userId)
        {
            RequestListWindow requestListWindow = new RequestListWindow(userId);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            requestListWindow.Show();
        }

        public void OpenSignUpWindow()
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            signUpWindow.Show();
        }

        public void OpenLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            loginWindow.Show();
        }

        public void OpenRoomDetailsWindow(int userId, int roomId, DateTime? bookDate)
        {
            RoomDetailsWindow roomDetailsWindow = new RoomDetailsWindow(userId, roomId, bookDate);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            roomDetailsWindow.Show();
        }

        public void OpenFindResultWindow(int userId, DateTime? bookingDate, TimeSpan? startTime, TimeSpan? endTime, int? departmentId, int? roomType)
        {
            FindResultWindow findResultWindow = new FindResultWindow(userId, bookingDate, startTime, endTime, departmentId, roomType);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
            findResultWindow.Show();
        }
    }
}
