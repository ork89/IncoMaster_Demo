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

            //GetLoggedUser();
        }

        private async Task GetLoggedUser()
        {
            LoggedUser = await CoreGrpcClient.GetUsersRequest();
            RaisePropertyChange("LoggedUser");
            if (_loggedUser != null)
            {
                UserName = $"{_loggedUser.FirstName} {_loggedUser.LastName}";
                UserBalance = _loggedUser.Balance;
                IncomeList = _loggedUser.IncomeList;
                ExpensesList = _loggedUser.ExpensesList;
                SavingsList = _loggedUser.SavingsList;
                LoansList = _loggedUser.LoansList;

                SelectedViewModel = new OverviewViewModel();
                RaisePropertyChange("SelectedViewModel");
            }
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
                if (value != _loggedUser)
                {
                    _loggedUser = value;
                    RaisePropertyChange();
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
                    RaisePropertyChange();
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
                    RaisePropertyChange();
                }
            }
        }

        private IList<CategoriesModel> _incomeList;

        public IList<CategoriesModel> IncomeList
        {
            get { return _incomeList; }
            set
            {
                if (value != _incomeList)
                {
                    _incomeList = value;
                    RaisePropertyChange();
                }
            }
        }

        private IList<CategoriesModel> _expensesList;

        public IList<CategoriesModel> ExpensesList
        {
            get { return _expensesList; }
            set 
            { 
                if(value != _expensesList)
                {
                    _expensesList = value;
                    RaisePropertyChange();
                }
            }
        }

        private IList<CategoriesModel> _savingsList;

        public IList<CategoriesModel> SavingsList
        {
            get { return _savingsList; }
            set 
            { 
                if(value != _savingsList)
                {
                    _savingsList = value;
                    RaisePropertyChange();
                }
            }
        }

        private IList<CategoriesModel> _loansList;

        public IList<CategoriesModel> LoansList
        {
            get { return _loansList; }
            set
            {
                if (value != _loansList)
                {
                    _loansList = value;
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
