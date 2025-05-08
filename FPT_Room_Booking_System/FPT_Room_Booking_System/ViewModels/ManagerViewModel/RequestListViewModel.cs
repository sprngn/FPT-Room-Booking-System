using FPT_Room_Booking_System.Models;
using FPT_Room_Booking_System.Resources;
using FPT_Room_Booking_System.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FPT_Room_Booking_System.ViewModels.ManagerViewModel
{
    public class RequestListViewModel : NotifyPropertyChangedBase
    {
        private ObservableCollection<Booking> _requests;
        private Booking _selectedRequest;
        private int _requestId;
        private readonly AppNavigationService nav;
        private int _userId;
        private string departmentName;
        private string roomName;
        private string roomType;

        public ObservableCollection<Booking> Requests
        {
            get => _requests;
            set => SetProperty(ref _requests, value);
        }

        public Booking SelectedRequest
        {
            get => _selectedRequest;
            set
            {
                if (SetProperty(ref _selectedRequest, value))
                {
                    RequestId = _selectedRequest?.BookingId ?? 0;

                    if (_selectedRequest != null)
                    {
                        LoadRoomDetails(_selectedRequest.RoomId);
                    }
                    else
                    {
                        DepartmentName = string.Empty;
                        RoomName = string.Empty;
                        RoomType = string.Empty;
                    }
                }
            }
        }

        public int RequestId
        {
            get => _requestId;
            set => SetProperty(ref _requestId, value);
        }

        public string DepartmentName
        {
            get => departmentName;
            set => SetProperty(ref departmentName, value);
        }

        public string RoomName
        {
            get => roomName;
            set => SetProperty(ref roomName, value);
        }

        public string RoomType
        {
            get => roomType;
            set => SetProperty(ref roomType, value);
        }


        public ICommand BtnBackCommand { get; }
        public ICommand AcceptRequestCommand { get; }
        public ICommand DeclineRequestCommand { get; }

        public RequestListViewModel(int userId)
        {
            _userId = userId;
            LoadData();
            BtnBackCommand = new RelayCommand(GoBack);
            AcceptRequestCommand = new RelayCommand(AcceptRequest, CanExecuteRequest);
            DeclineRequestCommand = new RelayCommand(DeclineRequest, CanExecuteRequest);
            nav = new AppNavigationService();
        }

        private void LoadRoomDetails(int roomId)
        {
            var room = context.Rooms
                .Include(r => r.Department)
                .Include(r => r.Type)
                .FirstOrDefault(r => r.RoomId == roomId);

            if (room != null)
            {
                DepartmentName = room.Department.DepartmentName;
                RoomName = room.RoomName;
                RoomType = room.Type.TypeName;
            }
            else
            {
                DepartmentName = "N/A";
                RoomName = "N/A";
                RoomType = "N/A";
            }
        }


        private void GoBack(object obj)
        {
            nav.OpenDepartmentWindow(_userId);
        }

        private void AcceptRequest(object obj)
        {
            if (SelectedRequest != null)
            {
                SelectedRequest.Status = "Booked";

                context.SaveChanges();

                MessageBox.Show($"Booking ID {SelectedRequest.BookingId} has been accepted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                SelectedRequest = null;
                RequestId = 0;

                LoadData();
            }
        }

        private void DeclineRequest(object obj)
        {
            if (SelectedRequest != null)
            {
                SelectedRequest.Status = "Cancelled";

                context.SaveChanges();

                MessageBox.Show($"Booking ID {SelectedRequest.BookingId} has been declined.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                SelectedRequest = null;
                RequestId = 0;

                LoadData();
            }
        }

        private bool CanExecuteRequest(object obj)
        {
            return SelectedRequest != null;
        }

        private void LoadData()
        {
            var bookings = context.Bookings
                .Include(b => b.BookingUserNavigation)
                .Include(b => b.SlotNavigation)
                .Include(b => b.Room)
                .Where(b => b.IsActive == true && b.Status.Equals("Pending"))
                .ToList();

            Requests = new ObservableCollection<Booking>(bookings);
        }
    }
}
