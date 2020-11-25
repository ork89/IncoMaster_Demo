using IncoMasterApp.Views;
using LiveCharts;
using LiveCharts.Wpf;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IncoMasterApp.ViewModels
{
    public class OverviewViewModel : BaseViewModel
    {
        private readonly MainWindowViewModel _mainVm;
        //private readonly UserModel _loggedUser;

        public OverviewViewModel(OverviewView overviewView = null)
        {
            IsLoading = true;
            _mainVm = MainWindowViewModel.Instance;
            User = _mainVm.LoggedUser;

            OverviewView = overviewView ?? new OverviewView();
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            if (User != null)
            {
                InitializeUserData(User);
                IsLoading = false;
            }

            SelectedMonth = DateTime.Today.Month;
            SelectedYear = DateTime.Today.Year;
        }

        #region Properties
        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set
            {
                if (value != _user)
                {
                    _user = value;
                    RaisePropertyChanged();
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (value != _isLoading)
                {
                    _isLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        private double _totalIncome;
        public double TotalIncome
        {
            get { return Math.Round(_totalIncome, 2); }
            set
            {
                if (value != _totalIncome)
                {
                    _totalIncome = value;
                    RaisePropertyChanged();
                }
            }
        }

        private double _totalExpenses;
        public double TotalExpenses
        {
            get { return Math.Round(_totalExpenses, 2); }
            set
            {
                if (value != _totalExpenses)
                {
                    _totalExpenses = value;
                    RaisePropertyChanged();
                }
            }
        }

        private double _totalSavings;
        public double TotalSavings
        {
            get { return Math.Round(_totalSavings, 2); }
            set
            {
                if (value != _totalSavings)
                {
                    _totalSavings = value;
                    RaisePropertyChanged();
                }
            }
        }

        private double _totalLoans;
        public double TotalLoans
        {
            get { return Math.Round(_totalLoans, 2); }
            set
            {
                if (value != _totalLoans)
                {
                    _totalLoans = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        private double totalValues;

        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }

        public List<int> Months { get { return base.MonthsList; } }
        public List<int> Years { get { return base.YearsList; } }

        public Func<ChartPoint, string> PointLabel { get; set; }
        private ObservableCollection<PieSeries> PieSeries { get; set; }
        public OverviewView OverviewView { get; set; }

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
        private void InitializeUserData(UserModel loggedUser)
        {
            GetUserNameAndCurrentBalance();
            GetTotalIncome();
            GetTotalExpenses();
            GetTotalSavings();
            GetTotalLoans();

            CalculateValuesForPieChart();

            if (OverviewView != null)
                UpdatePieChart();
        }

        private void GetUserNameAndCurrentBalance()
        {
            if (User != null)
            {
                UserName = $"{User.FirstName} {User.LastName}";
                Balance = Math.Round(User.Balance, 2);
            }
        }

        private void GetTotalIncome()
        {
            if (User.IncomeList != null)
            {
                foreach (var amount in User.IncomeList)
                {
                    TotalIncome += Math.Round(amount.Amount, 2);
                }
            }
        }

        private void GetTotalExpenses()
        {
            if (User.ExpensesList != null)
            {
                foreach (var amount in User.ExpensesList)
                {
                    TotalExpenses += Math.Round(amount.Amount, 2);
                }
            }
        }

        private void GetTotalSavings()
        {
            if (User.SavingsList != null)
            {
                foreach (var amount in User.SavingsList)
                {
                    TotalSavings += Math.Round(amount.Amount, 2);
                }
            }
        }

        private void GetTotalLoans()
        {
            if (User.LoansList != null)
            {
                foreach (var amount in User.LoansList)
                {
                    TotalLoans += Math.Round(amount.Amount, 2);
                }
            }
        }

        private void CalculateValuesForPieChart()
        {
            totalValues = TotalIncome + TotalExpenses + TotalSavings + TotalLoans;

            IncomeValue = (TotalIncome / totalValues) * 100;
            ExpensesValue = (TotalExpenses / totalValues) * 100;
            SavingsValue = (TotalSavings / totalValues) * 100;
            LoansValue = (TotalLoans / totalValues) * 100;
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

        #endregion Methods
    }
}
