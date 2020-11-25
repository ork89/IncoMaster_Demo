using DotNetCoreGrpcClient;
using Models;
using HelperClasses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Windows.Input;
using IncoMasterApp.Interfaces;
using System.Text;
using System.Security.Cryptography;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;

namespace IncoMasterApp.ViewModels
{
    public class RegistrationViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly IWindowService _windowService;
        private readonly Sha256Converter _converter;

        public RegistrationViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _converter = new Sha256Converter();

            //RegisterUserCommand = new RelayCommand(RegisterNewUser, param => this.CanExecute);
            RegisterUserCommand = new RelayCommand<Window>(this.RegisterNewUser);
            ClearRegistrationFormCommand = new RelayCommand(ClearFormFields, param => this.CanExecute);
        }


        #region Prorperties
        private string _firstName;
        [Required]
        [MinLength(2, ErrorMessage = "First name is required")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value != _firstName)
                {
                    _firstName = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _lastName;
        [Required]
        [MinLength(2, ErrorMessage = "Last name is required")]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value != _lastName)
                {
                    _lastName = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _email;
        [Required]
        public string Email
        {
            get { return _email; }
            set
            {
                if (value != _email)
                {
                    _email = value;
                    RaisePropertyChanged();
                }
            }
        }

        private SecureString _password;
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 Characters")]
        public SecureString Password
        {
            get { return _password; }
            set
            {
                if (value != _password)
                {
                    _password = value;
                    RaisePropertyChanged();
                }
            }
        }

        private SecureString _confPassword;
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 Characters")]
        public SecureString ConfPassword
        {
            get { return _confPassword; }
            set
            {
                if (value != _confPassword)
                {
                    _confPassword = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ExtraPass { get { return false; } }
        public bool ExtraConfPass { get { return false; } }

        public bool IsSubmitBtnEnabled { get; set; }
        public bool IsRegistrationSuccess { get; set; }

        string IDataErrorInfo.Error { get { return ValidatePasswords(null); } }
        string IDataErrorInfo.this[string columnName] { get { return ValidatePasswords(columnName); } }

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
        #endregion Prorperties

        #region Commands
        //public ICommand RegisterUserCommand { get; set; }
        public RelayCommand<Window> RegisterUserCommand { get; private set; }
        public ICommand ClearRegistrationFormCommand { get; set; }

        #endregion Commands

        #region Methods
        private async void RegisterNewUser(Window win)
        {
            var newUser = new UserModel
            {
                FirstName = FirstName,
                LastName = LastName,
                AccountType = "User",
                Email = Email,
                Password = _converter.HashSecureString(Password),
                Balance = 0
            };

            var result = await CoreGrpcClient.AddOrUpdateUser(newUser);

            if (result == "True")
            {
                ClearFormFields(result);
                IsRegistrationSuccess = true;
                //_windowService.CloseRegistration(new LoginViewModel(new WindowService()));
                if (win != null) win.Close();
            };
        }

        private void ClearFormFields(object obj)
        {
            if (!string.IsNullOrEmpty(FirstName)) FirstName = string.Empty;
            if (!string.IsNullOrEmpty(LastName)) LastName = string.Empty;
            if (!string.IsNullOrEmpty(Email)) Email = string.Empty;
        }

        public void SetPassword(SecureString pass)
        {
            _password = pass.Copy();
            _password.MakeReadOnly();
            RaisePropertyChanged("ExtraPass");
        }

        public void SetConfPassword(SecureString pass)
        {
            _confPassword = pass.Copy();
            _confPassword.MakeReadOnly();
            RaisePropertyChanged("ExtraConfPass");
        }

        public string ValidatePasswords(string propertyName)
        {
            string errorMsg = string.Empty;

            switch (propertyName)
            {
                case "ExtraPass":
                    if(Password == null || Password.Length == 0)
                    {
                        errorMsg = "Password is required";
                        break;
                    }
                    else if (Password.Length < 8)
                    {
                        errorMsg = "Password must be at least 8 characters";
                        break;
                    }
                    break;

                case "ExtraConfPass":
                    if (ConfPassword == null || ConfPassword.Length == 0)
                    {
                        errorMsg = "Password is required";
                        break;
                    }
                    else if (ConfPassword.Length < 8)
                    {
                        errorMsg = "Password must be at least 8 characters";
                        break;
                    }
                    else if(_converter.HashSecureString(ConfPassword) != (_converter.HashSecureString(Password)))
                    {
                        
                        errorMsg = "Passwords don't match";
                        break;
                    }
                    break;
                default:
                    break;
            }


            return errorMsg;
        }

        #endregion Methods
    }
}
