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
using System.Windows;
using System.Windows.Input;

namespace FPT_Room_Booking_System.ViewModels
{
    public class FindResultViewModel : NotifyPropertyChangedBase
    {
        private int _userId;
        private DateTime? _bookingDate;
        private TimeSpan? _startTime;
        private TimeSpan? _endTime;
        private int? _departmentId;
        private int? _roomType;
        private readonly AppNavigationService nav;

        public ObservableCollection<Room> BtnRoom { get; set; }
        public ICommand BtnBackCommand { get; set; }
        public ICommand SelectRoomCommand { get; set; }

        public FindResultViewModel(int userId, DateTime? bookingDate, TimeSpan? startTime, TimeSpan? endTime, int? departmentId, int? roomType)
        {
            _userId = userId;
            _bookingDate = bookingDate;
            _startTime = startTime;
            _endTime = endTime;
            _departmentId = departmentId;
            _roomType = roomType;

            nav = new AppNavigationService();
            BtnBackCommand = new RelayCommand(BackToPrevious);
            SelectRoomCommand = new RelayCommand(OnSelectRoom);

            LoadResult();
        }

        private void LoadResult()
        {
            var rooms = context.Rooms
                .Where(r =>
                    (!_departmentId.HasValue || r.DepartmentId == _departmentId) &&
                    (!_roomType.HasValue || r.TypeId == _roomType))
                .ToList();

            var availableRooms = rooms.Where(room =>
            {
                var bookings = context.Bookings
                    .Where(b =>
                        b.RoomId == room.RoomId &&
                        b.BookingDate == _bookingDate &&
                        b.IsActive == true)
                    .Select(b => b.SlotNavigation)
                    .ToList();

                return bookings.All(slot =>
                    slot.EndTime <= _startTime || slot.StartTime >= _endTime); 
            }).ToList();

            BtnRoom = new ObservableCollection<Room>(availableRooms);
            OnPropertyChanged(nameof(BtnRoom));
        }


        private void OnSelectRoom(object parameter)
        {
            if (parameter is Room selectedRoom)
            {
                nav.OpenRoomDetailsWindow(_userId, selectedRoom.RoomId, _bookingDate);
            }
            else
            {
                MessageBox.Show("No room selected");
            }
        }

        private void BackToPrevious(object obj)
        {
            nav.OpenDepartmentWindow(_userId);
        }
    }
}
