using FPT_Room_Booking_System.Models;
using FPT_Room_Booking_System.Resources;
using FPT_Room_Booking_System.Services;
using FPT_Room_Booking_System.Views;
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
    public class LoginViewModel : NotifyPropertyChangedBase
    {
        private string username;
        private string password;
        private ObservableCollection<string> campus;
        private string selectedCampus;
        private readonly AppNavigationService nav;


        public string Username { get => username; set => SetProperty(ref username, value); }

        public string Password { get => password; set => SetProperty(ref password, value); }

        public ObservableCollection<string> Campus { get => campus; set => SetProperty(ref campus, value); }

        public string SelectedCampus { get => selectedCampus; set => SetProperty(ref selectedCampus, value); }

        public ICommand LoginCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

        public LoginViewModel()
        {
            nav = new AppNavigationService();
            campus = new ObservableCollection<string> { "Select a campus", "FU-Hoa Lac", "FU-Can Tho", "FU-Da Nang", "FU-Quy Nhon", "FU-HCM" };
            selectedCampus = "FU-Hoa Lac";
            LoginCommand = new RelayCommand(ExecuteLogin);
            SignUpCommand = new RelayCommand(MoveToSignUp);
        }

        private void MoveToSignUp(object obj)
        {
            nav.OpenSignUpWindow();
        }

        private void ExecuteLogin(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("Please fill in all blanks", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var user = context.Users.FirstOrDefault(u => u.UserName == Username && u.Password == Password);
                if (user != null)
                {
                    MessageBox.Show("Login successful", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    nav.OpenDepartmentWindow(user.UserId);
                }
                else
                {
                    MessageBox.Show("Username or password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
