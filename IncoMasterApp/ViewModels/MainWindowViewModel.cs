using Models;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using DotNetCoreGrpcClient;
using System;
using System.Threading.Tasks;

namespace IncoMasterApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        CoreGrpcClient grpcClient;
        private static MainWindowViewModel _instance = new MainWindowViewModel();
        public static MainWindowViewModel Instance { get { return _instance; } }

        public MainWindowViewModel()
        {
            switchToHomeViewCommand = new RelayCommand(SwitchToHomeView, param => this.CanExecute);
            switchToIncomeViewCommand = new RelayCommand(SwitchToIncomeView, param => this.CanExecute);
            switchToExpensesViewCommand = new RelayCommand(SwitchToExpensesView, param => this.CanExecute);
            switchToSavingsViewCommand = new RelayCommand(SwitchToSavingsView, param => this.CanExecute);
            switchToLoansViewCommand = new RelayCommand(SwitchToLoansView, param => this.CanExecute);

            SelectedViewModel = new OverviewViewModel();

            GetLoggedUser();
        }

        private async Task GetLoggedUser()
        {
            LoggedUser = await CoreGrpcClient.GetUsersRequest();
            if(LoggedUser != null)
            {
                _userName = $"{LoggedUser.FirstName} {LoggedUser.LastName}";
                _userBalance = LoggedUser.Balance;
            }
        }

        #region Commands
        public ICommand switchToHomeViewCommand { get; set; }
        public ICommand switchToIncomeViewCommand { get; set; }
        public ICommand switchToExpensesViewCommand { get; set; }
        public ICommand switchToSavingsViewCommand { get; set; }
        public ICommand switchToLoansViewCommand { get; set; }
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
                    OnPropertyChanged("SelectedViewModel");
                }
            }
        }

        private UserModel _loggedUser;
        public UserModel LoggedUser
        {
            get { return _loggedUser; }
            set 
            { 
                if(value != _loggedUser)
                {
                    _loggedUser = value;
                    OnPropertyChanged("LoggedUser");
                }
                
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set 
            { 
                if(value != _userName)
                {
                    _userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        private double _userBalance;

        public double UserBalance
        {
            get { return _userBalance; }
            set 
            { 
                if(value != _userBalance)
                {
                    _userBalance = value;
                    OnPropertyChanged("UserBalance");
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

        #region Methods

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

        #endregion Methods
    }
}
