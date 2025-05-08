using FPT_Room_Booking_System.ViewModels.UserViewModel;
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

namespace FPT_Room_Booking_System.Views.UserWindow
{
    /// <summary>
    /// Interaction logic for MyBookingWindow.xaml
    /// </summary>
    public partial class MyBookingWindow : Window
    {
        public MyBookingWindow(int userId)
        {
            InitializeComponent();
            DataContext = new MyBookingViewModel(userId);
        }
    }
}
