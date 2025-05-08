using FPT_Room_Booking_System.Models;
using FPT_Room_Booking_System.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FPT_Room_Booking_System.Views
{
    /// <summary>
    /// Interaction logic for FindResultWindow.xaml
    /// </summary>
    public partial class FindResultWindow : Window
    {
        public FindResultWindow(int userId, DateTime? bookingDate, TimeSpan? startTime, TimeSpan? endTime, int? departmentId, int? roomType)
        {
            InitializeComponent();
            DataContext = new FindResultViewModel(userId, bookingDate, startTime, endTime, departmentId, roomType);
        }
    }
}
