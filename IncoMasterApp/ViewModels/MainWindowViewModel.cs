using GalaSoft.MvvmLight.Command;
using IncoMasterApp.Interfaces;
using MaterialDesignThemes.Wpf;
using Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace IncoMasterApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel(new WindowService());
        private readonly IWindowService _windowsService;

        public MainWindowViewModel(IWindowService windowService)
        {
            _windowsService = windowService;
            MainSnackbarMessage = new SnackbarMessage();

            SwitchToHomeViewCommand = new RelayCommand(SwitchToHomeView, param => this.CanExecute);
            SwitchToIncomeViewCommand = new RelayCommand(SwitchToIncomeView, param => this.CanExecute);
            SwitchToExpensesViewCommand = new RelayCommand(SwitchToExpensesView, param => this.CanExecute);
            SwitchToSavingsViewCommand = new RelayCommand(SwitchToSavingsView, param => this.CanExecute);
            SwitchToLoansViewCommand = new RelayCommand(SwitchToLoansView, param => this.CanExecute);
            LogoutUserCommand = new RelayCommand<Window>(LogoutUser);
            LogoutAndExitCommand = new RelayCommand<Window>(LogoutAndExit, param => this.CanExecute);
            CloseSnackbarCommand = new RelayCommand(CloseSnackbar, param => this.CanExecute);
        }

        #region Commands
        public ICommand SwitchToHomeViewCommand { get; set; }
        public ICommand SwitchToIncomeViewCommand { get; set; }
        public ICommand SwitchToExpensesViewCommand { get; set; }
        public ICommand SwitchToSavingsViewCommand { get; set; }
        public ICommand SwitchToLoansViewCommand { get; set; }
        public ICommand LogoutUserCommand { get; set; }
        public ICommand LogoutAndExitCommand { get; set; }
        public ICommand CloseSnackbarCommand { get; set; }
        #endregion Commands

        #region Properties

        private object _selectedViewModel;
        public object SelectedViewModel
        {
            get
            {
                if (_selectedViewModel == null)
                    _selectedViewModel = new OverviewViewModel();
                return _selectedViewModel;
            }
            set
            {
                if (value != _selectedViewModel)
                {
                    _selectedViewModel = value;
                    RaisePropertyChanged();
                }
            }
        }

        private UserModel _loggedUser;
        public UserModel LoggedUser
        {
            get { return _loggedUser; }
            set
            {
                if (value != _loggedUser)
                {
                    _loggedUser = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isProgressbarVisible;
        public bool IsProgressbarVisible
        {
            get { return _isProgressbarVisible; }
            set
            {
                if (value != _isProgressbarVisible)
                {
                    _isProgressbarVisible = value;
                    RaisePropertyChanged();
                }
            }
        }
        
        private SnackbarMessage _mainSnackbarMessage;
        public SnackbarMessage MainSnackbarMessage
        {
            get { return _mainSnackbarMessage; }
            set
            {
                if (value != _mainSnackbarMessage)
                {
                    _mainSnackbarMessage = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isSnackbarActive;
        public bool IsSnackbarActive
        {
            get { return _isSnackbarActive; }
            set
            {
                if (value != _isSnackbarActive)
                {
                    _isSnackbarActive = value;
                    RaisePropertyChanged();
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

        #endregion Properties

        #region CommandMethods
        private void LogoutUser(Window win)
        {
            //TODO: Clear data from LoggedUser and all inherted lists in all UserControls.
            //      Close MainWindow.
            Application.Current.Shutdown();
        }

        private void LogoutAndExit(object obj)
        {
            //TODO: Execute LogoutUser
            Application.Current.Shutdown();
        }

        private void SwitchToHomeView(object obj)
        {
            SelectedViewModel = new OverviewViewModel();
        }

        private void SwitchToIncomeView(object obj)
        {
            SelectedViewModel = new IncomeViewModel();
        }

        private void SwitchToExpensesView(object obj)
        {
            SelectedViewModel = new ExpensesViewModel();
        }

        private void SwitchToSavingsView(object obj)
        {
            SelectedViewModel = new SavingsViewModel();
        }

        private void SwitchToLoansView(object obj)
        {
            SelectedViewModel = new LoansViewModel();
        }

        private void DisplaySnackbar(string content)
        {            
            MainSnackbarMessage = new SnackbarMessage
            {
                ActionContent = "Bye",
                ActionCommand = CloseSnackbarCommand,
                Content = $"{content}",
            };

            IsSnackbarActive = true;
        }

        private void CloseSnackbar(object obj)
        {
            IsSnackbarActive = false;
        }
        #endregion CommandMethods

    }    
}
