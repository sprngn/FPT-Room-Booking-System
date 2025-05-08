using FPT_Room_Booking_System.Models;
using FPT_Room_Booking_System.Resources;
using FPT_Room_Booking_System.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace FPT_Room_Booking_System.ViewModels
{
    public class RoomDetailsViewModel : NotifyPropertyChangedBase
    {
        private int _userId;
        private int _roomId;
        private readonly AppNavigationService nav;
        private DateTime? bookingDate;
        private string departmentName;
        private string roomName;
        private string roomType;
        private int capacity;
        private string projector;
        private Slot selectedSlot;
        private ObservableCollection<Slot> allSlot;
        private ObservableCollection<Booking> roomBooking;

        public DateTime? BookingDate
        {
            get => bookingDate;
            set
            {
                if (SetProperty(ref bookingDate, value))
                {
                    loadData();
                }
            }
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

        public int Capacity
        {
            get => capacity;
            set => SetProperty(ref capacity, value);
        }

        public string Projector
        {
            get => projector;
            set => SetProperty(ref projector, value);
        }

        public Slot SelectedSlot
        {
            get => selectedSlot;
            set => SetProperty(ref selectedSlot, value);
        }

        public ObservableCollection<Slot> AllSlot
        {
            get => allSlot;
            set => SetProperty(ref allSlot, value);
        }

        public ObservableCollection<Booking> RoomBookings
        {
            get => roomBooking;
            set => SetProperty(ref roomBooking, value);
        }

        public ICommand RequestBookingCommand { get; }
        public ICommand BtnBackCommand { get; }

        public RoomDetailsViewModel(int userId, int roomId, DateTime? bookDate)
        {
            _userId = userId;
            _roomId = roomId;
            BookingDate = bookDate ?? DateTime.Now;
            loadData();
            RequestBookingCommand = new RelayCommand(RequestBooking);
            BtnBackCommand = new RelayCommand(BackToPreviousWindow);
            nav = new AppNavigationService();
        }

        private void loadData()
        {
            var selectedRoom = context.Rooms.Include(i => i.Department).Include(i => i.Type).Where(i => i.RoomId == _roomId).FirstOrDefault();
            if(selectedRoom != null)
            {
                DepartmentName = selectedRoom.Department.DepartmentName;
                RoomName = selectedRoom.RoomName;
                RoomType = selectedRoom.Type.TypeName;
                Capacity = selectedRoom.Type.Capacity;
                Projector = selectedRoom.Type.Projector == true ? "Yes" : "No";
            }

            var slots = context.Slots.ToList();
            AllSlot = new ObservableCollection<Slot>(slots);
            SelectedSlot = AllSlot.FirstOrDefault();

            if (BookingDate.HasValue)
            {
                var bookings = context.Bookings
                                      .Include(b => b.SlotNavigation)
                                      .Where(b => b.RoomId == _roomId && b.BookingDate.Date == BookingDate.Value.Date && b.IsActive == true && !b.Status.Equals("Cancelled"))
                                      .OrderBy(b => b.SlotNavigation.StartTime)
                                      .ToList();
                RoomBookings = new ObservableCollection<Booking>(bookings);
            }
            else
            {
                RoomBookings = new ObservableCollection<Booking>();
            }
        }

        private void RequestBooking(object parameter)
        {
            if (!ValidateBookingInputs()) return;

            if (HandleExistingBooking()) return;

            if (!ValidateUserRoleAndLimits()) return;

            CreateNewBooking();
        }

        private bool ValidateBookingInputs()
        {
            if (BookingDate.Value.Date < DateTime.Now.Date)
            {
                System.Windows.MessageBox.Show("Booking date cannot be in the past.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }

            var slotStartTime = SelectedSlot.StartTime;
            if (BookingDate.Value.Date == DateTime.Now.Date && slotStartTime < DateTime.Now.TimeOfDay)
            {
                System.Windows.MessageBox.Show("Selected slot's start time cannot be in the past.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool HandleExistingBooking()
        {
            var existingBooking = context.Bookings
                .Include(b => b.BookingUserNavigation)
                .Where(b => b.Slot == SelectedSlot.SlotId &&
                            b.RoomId == _roomId &&
                            b.BookingDate.Date == BookingDate.Value.Date &&
                            b.IsActive == true)
                .FirstOrDefault();

            if (existingBooking != null)
            {
                var bookingUserRole = existingBooking.BookingUserNavigation.Role;
                var currentUserRole = GetCurrentUserRole();

                if (currentUserRole == 2 && existingBooking.Status == "Booked")
                {
                    existingBooking.Status = "Cancelled";
                    existingBooking.IsActive = false;

                    context.SaveChanges();

                    CreateNewBooking();
                    return true;
                }

                if (currentUserRole == 3 && bookingUserRole == 4)
                {
                    UpdateBooking(existingBooking);
                    return true;
                }

                if (currentUserRole == 3 && bookingUserRole != 4 && bookingUserRole != 3)
                {
                    System.Windows.MessageBox.Show("You cannot update this booking as it belongs to another user.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return true;
                }

                System.Windows.MessageBox.Show("This booking already exists.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return true;
            }

            return false;
        }



        private bool ValidateUserRoleAndLimits()
        {
            var currentUserRole = GetCurrentUserRole();

            if (currentUserRole == 4)
            {
                var userBookingCount = context.Bookings
                    .Count(b => b.BookingUser == _userId &&
                                b.Status != "Cancelled" &&
                                b.BookingDate.Date >= DateTime.Now.Date);

                if (userBookingCount >= 2)
                {
                    System.Windows.MessageBox.Show("You cannot have more than 2 active bookings.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        private void CreateNewBooking()
        {
            var currentUserRole = GetCurrentUserRole();

            var bookingStatus = currentUserRole == 2 ? "Booked" : "Pending";

            var newBooking = new Booking
            {
                RoomId = _roomId,
                BookingUser = _userId,
                BookingDate = BookingDate.Value.Date,
                Slot = SelectedSlot.SlotId,
                Status = bookingStatus,
                IsActive = true
            };

            try
            {
                context.Bookings.Add(newBooking);
                context.SaveChanges();

                RoomBookings.Add(newBooking);
                System.Windows.MessageBox.Show($"Booking with status '{bookingStatus}' created successfully!", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                loadData();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while creating the booking: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void UpdateBooking(Booking existingBooking)
        {
            existingBooking.BookingUser = _userId;

            try
            {
                context.SaveChanges();
                System.Windows.MessageBox.Show("Booking successfully updated.", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                loadData();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"An error occurred while updating the booking: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private int GetCurrentUserRole()
        {
            return context.Users
                .Where(u => u.UserId == _userId)
                .Select(u => u.Role)
                .FirstOrDefault();
        }

        private void BackToPreviousWindow(object parameter)
        {
            var selectedRoom = context.Rooms.Where(r => r.RoomId == _roomId).FirstOrDefault();
            nav.OpenRoomWindow(selectedRoom.DepartmentId, _userId);
        }
    }
}
