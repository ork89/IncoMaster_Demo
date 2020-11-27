using IncoMasterApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IncoMasterApp.Interfaces
{
    public interface IWindowService
    {
        void OpenRegistration(RegistrationViewModel vm);
        void OpenMainWindow(MainWindowViewModel mvm);
        void OpenLogin(LoginViewModel lvm);
    }
}
