using IncoMasterApp.Interfaces;
using IncoMasterApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DotNetCoreGrpcClient;
using System.Security;
using HelperClasses;
using GalaSoft.MvvmLight.CommandWpf;

namespace IncoMasterApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IWindowService _windowService;
        private readonly Sha256Converter _converter;
        public LoginViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _converter = new Sha256Converter();

            RegisterNewUserCommand = new RelayCommand(RegisterNewUser, param => this.CanExecute);
            LoginCommand = new RelayCommand<Window>(LoginUser);
        }

        #region Commands
        public ICommand RegisterNewUserCommand { get; set; }
        public ICommand LoginUserCommand { get; set; }
        public RelayCommand<Window> LoginCommand { get; private set; }
        #endregion

        #region Properties

        private string _email;

        public string Email
        {
            get { return _email; }
            set 
            { 
                if(value != _email)
                {
                    _email = value;
                    RaisePropertyChange();
                }
            }
        }

        private SecureString _password;

        public SecureString Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool ExtraLoginPass { get { return false; } }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            { 
                if(value != _errorMessage)
                {
                    _errorMessage = value;
                    RaisePropertyChange();
                }
            }
        }


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
        #endregion

        #region Methods
        private void RegisterNewUser(object obj)
        {
            RegistrationViewModel reg = new RegistrationViewModel(new WindowService());
            _windowService.OpenRegistration(reg);
        }

        private async void LoginUser(Window win)
        {
            MainWindowViewModel mVm = MainWindowViewModel.Instance;
            OverviewViewModel oVm = new OverviewViewModel();

            var loggedUser = mVm.LoggedUser;
            loggedUser = await CoreGrpcClient.LoginUser(Email, _converter.HashSecureString(Password));
            RaisePropertyChange("LoggedUser");

            if (loggedUser != null)
            {
                mVm.SelectedViewModel = oVm;
                RaisePropertyChange("SelectedViewModel");

                mVm.LoggedUser = loggedUser;

                if(win != null) win.Close();
            }
            else
            {
                ErrorMessage = "Email or password are incorrect.";
            }
        }

        public void SetPassword(SecureString pass)
        {
            _password = pass.Copy();
            _password.MakeReadOnly();
            RaisePropertyChange("ExtraLoginPass");
        }
        #endregion
    }
}
