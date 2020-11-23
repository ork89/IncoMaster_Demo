using Models;
using System;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using IncoMasterApp.Views;
using System.Collections.ObjectModel;
using IncoMasterApp.Interfaces;

namespace IncoMasterApp.ViewModels
{
    public class OverviewViewModel : BaseViewModel
    {
        private static OverviewViewModel _overViewInstance = new OverviewViewModel();
        public static OverviewViewModel OverViewInstance { get { return _overViewInstance; } }

        private readonly MainWindowViewModel _mainVm;
        private readonly UserModel _loggedUser;

        public OverviewViewModel(OverviewView overviewView = null)
        {
            _mainVm = MainWindowViewModel.Instance;
            _loggedUser = _mainVm.LoggedUser;
            OverviewView = overviewView;

            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            if(_loggedUser != null && _loggedUser.Income != null)
                InitializeUserData(_loggedUser);
        }

        #region Properties
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set
            {
                if (value != _balance)
                {
                    _balance = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _totalIncome;
        public double TotalIncome
        {
            get { return _totalIncome; }
            set
            {
                if (value != _totalIncome)
                {
                    _totalIncome = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _totalExpenses;
        public double TotalExpenses
        {
            get { return _totalExpenses; }
            set
            {
                if (value != _totalExpenses)
                {
                    _totalExpenses = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _totalSavings;
        public double TotalSavings
        {
            get { return _totalSavings; }
            set
            {
                if (value != _totalSavings)
                {
                    _totalSavings = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _totalLoans;
        public double TotalLoans
        {
            get { return _totalLoans; }
            set
            {
                if (value != _totalLoans)
                {
                    _totalLoans = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _incomeValue;
        public double IncomeValue
        {
            get { return _incomeValue; }
            set
            {
                if (value != _incomeValue)
                {
                    _incomeValue = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _expensesValue;
        public double ExpensesValue
        {
            get { return _expensesValue; }
            set
            {
                if (value != _expensesValue)
                {
                    _expensesValue = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _savingsValue;
        public double SavingsValue
        {
            get { return _savingsValue; }
            set
            {
                if (value != _savingsValue)
                {
                    _savingsValue = value;
                    RaisePropertyChange();
                }
            }
        }

        private double _loansValue;
        public double LoansValue
        {
            get { return _loansValue; }
            set
            {
                if (value != _loansValue)
                {
                    _loansValue = value;
                    RaisePropertyChange();
                }
            }
        }

        private double totalValues;

        public Func<ChartPoint, string> PointLabel { get; set; }
        private ObservableCollection<PieSeries> PieSeries { get; set; }
        public OverviewView OverviewView { get; }

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
        private void InitializeUserData(UserModel loggedUser)
        {
            if (_loggedUser != null)
            {
                GetUserNameAndCurrentBalance();
                GetTotalIncome();
                GetTotalExpenses();
                GetTotalSavings();
                GetTotalLoans();
                
                CalculateValuesForPieChart();
                RaisePropertyChange();

                if (OverviewView != null)
                    UpdatePieChart();
            }
        }

        private void GetUserNameAndCurrentBalance()
        {
            if (_loggedUser != null)
            {
                UserName = $"{_loggedUser.FirstName} {_loggedUser.LastName}";
                Balance = Math.Round(_loggedUser.Balance, 2);
            }
        }

        private void GetTotalIncome()
        {
            foreach (var amount in _loggedUser.IncomeList)
            {
                TotalIncome += amount.Amount;
            }
        }

        private void GetTotalExpenses()
        {
            foreach (var amount in _loggedUser.ExpensesList)
            {
                TotalExpenses += Math.Round(amount.Amount, 2);
            }
        }

        private void GetTotalSavings()
        {
            foreach (var amount in _loggedUser.SavingsList)
            {
                TotalSavings += Math.Round(amount.Amount, 2);
            }
        }

        private void GetTotalLoans()
        {
            foreach (var amount in _loggedUser.LoansList)
            {
                TotalLoans += Math.Round(amount.Amount, 2);
            }
        }

        private void CalculateValuesForPieChart()
        {
            totalValues = TotalIncome + TotalExpenses + TotalSavings + TotalLoans;

            IncomeValue = (_totalIncome / totalValues) * 100;
            ExpensesValue = (_totalExpenses / totalValues) * 100;
            SavingsValue = (_totalSavings / totalValues) * 100;
            LoansValue = (_totalLoans / totalValues) * 100;
        }

        public void UpdatePieChart()
        {
            PieSeries = new ObservableCollection<PieSeries>
            {
                new PieSeries { Title = "Income", StrokeThickness = 0, Values = new ChartValues<double> { TotalIncome }, DataLabels = true, LabelPoint = PointLabel},
                new PieSeries { Title = "Expenses", StrokeThickness = 0, Values = new ChartValues<double> { TotalExpenses }, DataLabels = true, LabelPoint = PointLabel },
                new PieSeries { Title = "Savings", StrokeThickness = 0, Values = new ChartValues<double> { TotalSavings }, DataLabels = true, LabelPoint = PointLabel },
                new PieSeries { Title = "Loans", StrokeThickness = 0, Values = new ChartValues<double> { TotalLoans }, DataLabels = true, LabelPoint = PointLabel }
            };

            OverviewView.Chart.Series.AddRange(PieSeries);
        }

        #endregion
    }
}
