using Models;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using DotNetCoreGrpcClient;
using System;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace IncoMasterApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static MainWindowViewModel Instance { get; } = new MainWindowViewModel();

        public MainWindowViewModel()
        {
            SwitchToHomeViewCommand = new RelayCommand(SwitchToHomeView, param => this.CanExecute);
            SwitchToIncomeViewCommand = new RelayCommand(SwitchToIncomeView, param => this.CanExecute);
            SwitchToExpensesViewCommand = new RelayCommand(SwitchToExpensesView, param => this.CanExecute);
            SwitchToSavingsViewCommand = new RelayCommand(SwitchToSavingsView, param => this.CanExecute);
            SwitchToLoansViewCommand = new RelayCommand(SwitchToLoansView, param => this.CanExecute);
            OpenUserMenuCommand = new RelayCommand(OpenUserMenu, param => this.CanExecute);
            OpenSettingsMenuCommand = new RelayCommand(OpenSettingsMenu, param => this.CanExecute);
            LogoutUserCommand = new RelayCommand(LogoutUser, param => this.CanExecute);
        }

        #region Commands
        public ICommand SwitchToHomeViewCommand { get; set; }
        public ICommand SwitchToIncomeViewCommand { get; set; }
        public ICommand SwitchToExpensesViewCommand { get; set; }
        public ICommand SwitchToSavingsViewCommand { get; set; }
        public ICommand SwitchToLoansViewCommand { get; set; }
        public ICommand OpenUserMenuCommand { get; set; }
        public ICommand OpenSettingsMenuCommand { get; set; }
        public ICommand LogoutUserCommand { get; set; }
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
                if(value != _selectedViewModel)
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

        private void OpenUserMenu(object obj)
        {
            throw new NotImplementedException();
        }

        private void OpenSettingsMenu(object obj)
        {
            throw new NotImplementedException();
        }

        private void LogoutUser(object obj)
        {
            throw new NotImplementedException();
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

        #endregion CommandMethods
    }
}
