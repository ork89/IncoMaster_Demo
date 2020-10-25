using System;
using System.ComponentModel;
using System.Windows.Input;

namespace IncoMasterApp.ViewModels
{
    public class NavigationViewModel : INotifyPropertyChanged
    {
        public NavigationViewModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
