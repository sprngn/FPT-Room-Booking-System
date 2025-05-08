using FPT_Room_Booking_System.Models;
using FPT_Room_Booking_System.Resources;
using FPT_Room_Booking_System.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FPT_Room_Booking_System.ViewModels
{
    public class SignUpViewModel : NotifyPropertyChangedBase
    {
        private string username;
        private string email;
        private string password;
        private string confirmPassword;
        private readonly AppNavigationService nav;


        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public ICommand SignUpCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public SignUpViewModel()
        {
            nav = new AppNavigationService();
            SignUpCommand = new RelayCommand(ExecuteSignUp);
            CancelCommand = new RelayCommand(BackToLogin);
        }

        private void BackToLogin(object obj)
        {
            nav.OpenLoginWindow();
        }

        private void ExecuteSignUp(object parameter)
        {
            if (!IsValidInput())
                return;

            try
            {
                var newUser = new User
                {
                    UserName = Username,
                    Email = Email,
                    Password = Password,
                    Role = 4, 
                    Dob = null,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                using (var context = new FPT_Room_Booking_SystemContext())
                {
                    context.Users.Add(newUser);
                    context.SaveChanges();
                }

                MessageBox.Show("User created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Regex.IsMatch(Username, @"[^a-zA-Z0-9]"))
            {
                MessageBox.Show("Username should not contain special characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                MessageBox.Show("Email must be in the format: name@gmail.com.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Password.Length < 8 || Password.Length > 32)
            {
                MessageBox.Show("Password must be between 8 and 32 characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            bool usernameExists = context.Users.Any(u => u.UserName == Username);
            bool emailExists = context.Users.Any(u => u.Email == Email);

            if (usernameExists)
            {
                MessageBox.Show("The username is already taken. Please choose a different one.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (emailExists)
            {
                MessageBox.Show("The email is already registered. Please use a different email.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

    }
}
