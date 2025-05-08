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
    /// Interaction logic for ListRoomWindow.xaml
    /// </summary>
    public partial class ListRoomWindow : Window
    {
        public ListRoomWindow(int departmentId , int userId)
        {
            InitializeComponent();
            DataContext = new ListRoomViewModel(departmentId, userId);
        }
    }
}
