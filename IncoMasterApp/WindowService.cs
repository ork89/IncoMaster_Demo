using IncoMasterApp.Interfaces;
using IncoMasterApp.ViewModels;
using IncoMasterApp.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace IncoMasterApp
{
    public class WindowService : IWindowService
    {
        public void OpenRegistration(RegistrationViewModel vm)
        {
            RegistrationWindow reg = new RegistrationWindow();
            
            reg.DataContext = vm;
            reg.ShowDialog();
        }
    }
}
