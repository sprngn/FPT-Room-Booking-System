using FPT_Room_Booking_System.Models;
using FPT_Room_Booking_System.Resources;
using FPT_Room_Booking_System.Services;
using FPT_Room_Booking_System.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FPT_Room_Booking_System.ViewModels
{
    public class HomePageViewModel : NotifyPropertyChangedBase
    {
        private int _userId;
        private int userRole;
        private readonly AppNavigationService nav;
        private ObservableCollection<Department> btnDepartment;
        private ObservableCollection<Department> cbDepartment;
        private ObservableCollection<Slot> slotTime;
        private ObservableCollection<RoomType> roomType;

        private DateTime? dpBookDate;
        private Slot selectedStartTime;
        private Slot slectedEndTime;
        private Department selectedCbDepartment;
        private Department selectedBtnDepartment;
        private RoomType selectedRoomType;

        public ObservableCollection<Slot> SlotTime { get => slotTime; set => SetProperty(ref slotTime, value); }
        public ObservableCollection<Department> BtnDepartments { get => btnDepartment; set => SetProperty(ref btnDepartment, value); }
        public ObservableCollection<Department> CbDepartments { get => cbDepartment; set => SetProperty(ref cbDepartment, value); }
        public ObservableCollection<RoomType> RoomType { get => roomType; set => SetProperty(ref roomType, value); }

        public int UserRole { get => userRole; set { if (SetProperty(ref userRole, value)) { OnPropertyChanged(nameof(IsManager)); } } }
        public DateTime? SelectedBookDate { get => dpBookDate; set => SetProperty(ref dpBookDate, value); }
        public Slot SelectedStartTime { get => selectedStartTime; set => SetProperty(ref selectedStartTime, value); }
        public Slot SelectedEndTime { get => slectedEndTime; set => SetProperty(ref slectedEndTime, value); }
        public Department SelectedCbDepartment { get => selectedCbDepartment; set => SetProperty(ref selectedCbDepartment, value); }
        public Department SelectedBtnDepartment { get => selectedBtnDepartment; set => SetProperty(ref selectedBtnDepartment, value); }
        public RoomType SelectedRoomType { get => selectedRoomType; set => SetProperty(ref selectedRoomType, value); }
        public ICommand FindCommand { get; set; }
        public ICommand SelectDepartmentCommand { get; set; }
        public ICommand RequestListCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand MyBookingCommand { get; set; }

        public HomePageViewModel(int userId)
        {
            _userId = userId;
            LoadData();
            nav = new AppNavigationService();
            FindCommand = new RelayCommand(FindRoom);
            SelectDepartmentCommand = new RelayCommand(FindRoomInDepartment);
            RequestListCommand = new RelayCommand(ToRequestWindow);
            LogOutCommand = new RelayCommand(LogOutExecute);
            MyBookingCommand = new RelayCommand(ToMyBookingWindow);
        }

        private void ToMyBookingWindow(object obj)
        {
            nav.OpenMyBookingWindow(_userId);
        }

        private void ToRequestWindow(object obj)
        {
            nav.OpenRequestListWindow(_userId);
        }

        private void LogOutExecute(object obj)
        {
            nav.OpenLoginWindow();
        }

        public bool IsManager => UserRole == 2;

        private void FindRoomInDepartment(object parameter)
        {
            if (parameter is Department selectedDepartment)
            {
                nav.OpenRoomWindow(selectedDepartment.DepartmentId, _userId);
            }
            else
            {
                MessageBox.Show("No department selected.");
            }
        }


        private void FindRoom(object parameter)
        {
            if (SelectedEndTime.EndTime <= SelectedStartTime.StartTime)
            {
                MessageBox.Show("End time cannot be earlier than or equal to start time.", "Invalid Time Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? departmentFilter = SelectedCbDepartment?.DepartmentId == 0 ? (int?)null : SelectedCbDepartment?.DepartmentId;
            int? roomTypeFilter = SelectedRoomType?.TypeId == 0 ? (int?)null : SelectedRoomType?.TypeId;
            DateTime? bookDateFilter = dpBookDate?.Date ?? DateTime.Now.Date;

            nav.OpenFindResultWindow(_userId, bookDateFilter, SelectedStartTime.StartTime, SelectedEndTime.EndTime, departmentFilter, roomTypeFilter);
        }



        private void LoadData()
        {
            var departments = context.Departments
                                      .Where(d => d.IsActive == true)
                                      .ToList();
            BtnDepartments = new ObservableCollection<Department>(departments);

            var dp = context.Departments
                                      .Where(d => d.IsActive == true)
                                      .ToList();
            dp.Insert(0, new Department { DepartmentId = 0, DepartmentName = "Any Department" });
            CbDepartments = new ObservableCollection<Department>(dp);

            var slots = context.Slots.ToList();
            SlotTime = new ObservableCollection<Slot>(slots);

            var roomTypes = context.RoomTypes
                                   .Where(rt => rt.IsActive == true)
                                   .ToList();
            roomTypes.Insert(0, new RoomType { TypeId = 0, TypeName = "Any Room Type" });
            RoomType = new ObservableCollection<RoomType>(roomTypes);

            var user = context.Users
                .FirstOrDefault(u => u.UserId == _userId);
            UserRole = user.Role;

            SelectedCbDepartment = CbDepartments.FirstOrDefault();
            SelectedStartTime = SlotTime.FirstOrDefault();
            SelectedEndTime = SlotTime.FirstOrDefault();
            SelectedRoomType = RoomType.FirstOrDefault();
        }
    }
}