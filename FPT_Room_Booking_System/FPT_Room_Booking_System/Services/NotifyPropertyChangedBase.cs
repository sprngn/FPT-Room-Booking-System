using FPT_Room_Booking_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FPT_Room_Booking_System.Resources
{
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public FPT_Room_Booking_SystemContext context = new FPT_Room_Booking_SystemContext();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T Value, [CallerMemberName] string propertyName = null)
        {
            if(Equals(field, Value)) return false;

            field = Value;
            OnPropertyChanged(propertyName);
            return true;
        }
    } 
}
