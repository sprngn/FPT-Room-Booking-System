using FPT_Room_Booking_System.Models;
using FPT_Room_Booking_System.Resources;
using FPT_Room_Booking_System.Services;
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
    public class ListRoomViewModel : NotifyPropertyChangedBase
    {
        private int _userId;
        private int _departmentId;
        private ObservableCollection<RoomType> btnRoomType;
        private ObservableCollection<Room> btnRoom;
        private readonly AppNavigationService nav;

        private RoomType selectedRoomType;

        public ObservableCollection<RoomType> BtnRoomType { get => btnRoomType; set => SetProperty(ref btnRoomType, value); }
        public ObservableCollection<Room> BtnRoom { get => btnRoom; set => SetProperty(ref btnRoom, value); }
        public RoomType SelectedRoomType { get => selectedRoomType; set => SetProperty(ref selectedRoomType, value); }
        public ICommand SelectRoomTypeCommand { get; set; }
        public ICommand BtnBackCommand { get; set; }
        public ICommand SelectRoomCommand { get; set; }

        public ListRoomViewModel(int departmentId, int userId)
        {
            nav = new AppNavigationService();   
            _departmentId = departmentId;
            _userId = userId;
            loadData();
            SelectRoomTypeCommand = new RelayCommand(loadRoomForRoomType);
            BtnBackCommand = new RelayCommand(backToHome);
            SelectRoomCommand = new RelayCommand(selectRoom);
        }

        private void selectRoom(object parameter)
        {
            if(parameter is Room selectedRoom)
            {
                nav.OpenRoomDetailsWindow(_userId, selectedRoom.RoomId, DateTime.Now);
            }
            else
            {
                MessageBox.Show("No room selected");
            }
        }

        private void backToHome(object obj)
        {
            nav.OpenDepartmentWindow(_userId);
        }

        private void loadData()
        {
            var roomTypes = context.RoomTypes
                       .Where(rt => rt.IsActive == true)
                       .ToList();
            BtnRoomType = new ObservableCollection<RoomType>(roomTypes);
        }

        private void loadRoomForRoomType(object parameter)
        {
            if (parameter is RoomType roomType)
            {
                SelectedRoomType = roomType;

                var rooms = context.Rooms
                                   .Where(r => r.IsActive == true && r.TypeId == SelectedRoomType.TypeId && r.DepartmentId == _departmentId)
                                   .ToList();
                BtnRoom = new ObservableCollection<Room>(rooms);
            }
        }

    }
}
