using FPT_Room_Booking_System.Models;
using FPT_Room_Booking_System.Resources;
using FPT_Room_Booking_System.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FPT_Room_Booking_System.ViewModels.UserViewModel
{
    public class MyBookingViewModel : NotifyPropertyChangedBase
    {
        private int _userId;
        private readonly AppNavigationService nav;
        private ObservableCollection<Booking> _booking;

        public ObservableCollection<Booking> Bookings
        {
            get => _booking;
            set => SetProperty(ref _booking, value);
        }

        public ICommand BtnBackCommand { get; set; }

        public MyBookingViewModel(int userId)
        {
            _userId = userId;
            LoadData();
            BtnBackCommand = new RelayCommand(GoBack);
            nav = new AppNavigationService();
        }

        private void LoadData()
        {
            var bookings = context.Bookings
                .Include(b => b.BookingUserNavigation)
                .Include(b => b.SlotNavigation)
                .Include(b => b.Room)
                .ThenInclude(r => r.Department)
                .Include(b => b.Room)
                .ThenInclude(r => r.Type)
                .Where(b => b.IsActive == true && b.BookingUser == _userId)
                .ToList();

            Bookings = new ObservableCollection<Booking>(bookings);
        }

        private void GoBack(object obj)
        {
            nav.OpenDepartmentWindow(_userId);
        }
    }
}
