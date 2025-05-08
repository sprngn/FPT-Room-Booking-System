using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace FPT_Room_Booking_System.Resources
{
    public static class PasswordBoxBehavior
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxBehavior),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        // DependencyProperty for enabling or disabling the behavior
        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindPassword",
                typeof(bool),
                typeof(PasswordBoxBehavior),
                new PropertyMetadata(false, OnBindPasswordChanged));

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        public static bool GetBindPassword(DependencyObject obj)
        {
            return (bool)obj.GetValue(BindPasswordProperty);
        }

        public static void SetBindPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(BindPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox && !passwordBox.Password.Equals(e.NewValue))
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                passwordBox.Password = e.NewValue as string ?? string.Empty;
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                var wasBound = (bool)e.OldValue;
                var needToBind = (bool)e.NewValue;

                if (wasBound)
                {
                    passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                }

                if (needToBind)
                {
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBoundPassword(passwordBox, passwordBox.Password);
            }
        }
    }
}
