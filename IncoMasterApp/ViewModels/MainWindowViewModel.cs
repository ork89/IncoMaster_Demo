using Models;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using DotNetCoreGrpcClient;
using System;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Threading;

namespace IncoMasterApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        CoreGrpcClient grpcClient;
        private static MainWindowViewModel _instance = new MainWindowViewModel();
        public static MainWindowViewModel Instance { get { return _instance; } }

        public MainWindowViewModel()
        {
            SwitchToHomeViewCommand = new RelayCommand(SwitchToHomeView, param => this.CanExecute);
            SwitchToIncomeViewCommand = new RelayCommand(SwitchToIncomeView, param => this.CanExecute);
            SwitchToExpensesViewCommand = new RelayCommand(SwitchToExpensesView, param => this.CanExecute);
            SwitchToSavingsViewCommand = new RelayCommand(SwitchToSavingsView, param => this.CanExecute);
            SwitchToLoansViewCommand = new RelayCommand(SwitchToLoansView, param => this.CanExecute);
        }

        #region Commands
        public ICommand SwitchToHomeViewCommand { get; set; }
        public ICommand SwitchToIncomeViewCommand { get; set; }
        public ICommand SwitchToExpensesViewCommand { get; set; }
        public ICommand SwitchToSavingsViewCommand { get; set; }
        public ICommand SwitchToLoansViewCommand { get; set; }
        #endregion Commands

        #region Properties

        private object _selectedViewModel;
        public object SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                if(value != _selectedViewModel)
                {
                    _selectedViewModel = value;
                    RaisePropertyChange();
                }
            }
        }

        private UserModel _loggedUser;
        public UserModel LoggedUser
        {
            get { return _loggedUser; }
            set
            {
                if (LoggedUser == null && value != null)
                    this.SwitchToHomeView(null);

                if (value != _loggedUser)
                {
                    _loggedUser = value;
                    RaisePropertyChange();
                }
            }
        }

        public bool IsLoading { get; set; }

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
