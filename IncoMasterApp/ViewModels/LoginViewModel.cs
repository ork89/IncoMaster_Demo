using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace IncoMasterApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            RegisterNewUserCommand = new RelayCommand(RegisterNewUser, param => this.CanExecute);
            LoginUserCommand = new RelayCommand(LoginUser, param => this.CanExecute);
        }

        #region Commands
        ICommand RegisterNewUserCommand { get; set; }
        ICommand LoginUserCommand { get; set; }
        #endregion

        private bool _canExecute = true;
        public bool CanExecute
        {
            get
            {
                return this._canExecute;
            }

            set
            {
                if (value != this._canExecute)
                    this._canExecute = value;
            }
        }

        #region CommandMethods
        private void RegisterNewUser(object obj)
        {
            throw new NotImplementedException();
        }

        private void LoginUser(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
