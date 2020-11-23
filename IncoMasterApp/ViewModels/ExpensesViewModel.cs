using DotNetCoreGrpcClient;
using MaterialDesignThemes.Wpf;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace IncoMasterApp.ViewModels
{
    public class ExpensesViewModel : BaseViewModel
    {
        private const string DialogIdentifier = "RootDialogHost";

        public ExpensesViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;
            _expensesList = new ObservableCollection<CategoriesModel>();

            if (LoggedUser != null)
                InitExpensesList(LoggedUser.ExpensesList);

            AddExpensesCommand = new RelayCommand(AddNewExpenses, param => this.CanExecute);
            EditExpensesCommand = new RelayCommand(EditExpenses, param => this.CanExecute);
            DeleteExpensesCommand = new RelayCommand(DeleteExpenses, param => this.CanExecute);
            CloseSnackbarCommand = new RelayCommand(CloseSnackbar, param => this.CanExecute);

            InitExpensesTypesList();
            ExpensesSubmitDate = DateTime.Today.Date;
            IncomeSnackbarMessage = new SnackbarMessage();
        }

        #region Properties

        private ObservableCollection<CategoriesModel> _expensesList;
        public ObservableCollection<CategoriesModel> ExpensesList
        {
            get { return _expensesList; }
            set
            {
                if (value != _expensesList)
                {
                    _expensesList = value;
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

        private ObservableCollection<string> _expensesTypes;
        public ObservableCollection<string> ExpensesTypes
        {
            get { return _expensesTypes; }
            set
            {
                if (value != _expensesTypes)
                {
                    _expensesTypes = value;
                    RaisePropertyChange();
                }
            }
        }

        private string _selectedExpensesType;
        public string SelectedExpensesType
        {
            get { return _selectedExpensesType; }
            set
            {
                if (value != _selectedExpensesType)
                {
                    _selectedExpensesType = value;
                    RaisePropertyChange();
                }
            }
        }

        private CategoriesModel _selectedRow;
        public CategoriesModel SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                if (value != _selectedRow)
                {
                    _selectedRow = value;
                    RaisePropertyChange();
                }
            }
        }


        private DateTime _submitDate;
        public DateTime ExpensesSubmitDate
        {
            get { return _submitDate; }
            set
            {
                if (value != _submitDate)
                {
                    _submitDate = value.Date;
                    RaisePropertyChange();
                }
            }
        }

        private double _amount;
        public double ExpensesAmount
        {
            get { return _amount; }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    RaisePropertyChange();
                }
            }
        }

        private SnackbarMessage _incomeSnackbarMessage;
        public SnackbarMessage IncomeSnackbarMessage
        {
            get { return _incomeSnackbarMessage; }
            set
            {
                if (value != _incomeSnackbarMessage)
                {
                    _incomeSnackbarMessage = value;
                    RaisePropertyChange();
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
                    RaisePropertyChange();
                }
            }
        }

        //public event EventHandler Close;
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

        #region Commands
        public ICommand AddExpensesCommand { get; set; }
        public ICommand EditExpensesCommand { get; set; }
        public ICommand DeleteExpensesCommand { get; set; }
        public ICommand CloseSnackbarCommand { get; set; }
        #endregion Commands

        #region Methods

        private void InitExpensesList(List<CategoriesModel> expensesList)
        {
            foreach (var expense in expensesList)
            {
                ExpensesList.Add(expense);
            }
        }

        private async void AddNewExpenses(object obj)
        {
            object dialogResult = await DialogHost.Show(this, DialogIdentifier);

            if (dialogResult is bool boolResult && boolResult)
            {
                var newCategory = new CategoriesModel
                {
                    Category = "Expenses",
                    Title = SelectedExpensesType,
                    Amount = ExpensesAmount,
                    SubmitDate = ExpensesSubmitDate
                };

                ExpensesList.Add(newCategory);

                var result = await CoreGrpcClient.AddCategory(newCategory, LoggedUser.Id);

                //if result is empty it means that theres no error.
                if (string.IsNullOrEmpty(result))
                {
                    DisplaySnackbar("Added to your expenses");
                }
            }
        }

        private async void EditExpenses(object obj)
        {
            if (SelectedRow == null) return;

            SelectedExpensesType = SelectedRow.Title;
            ExpensesAmount = SelectedRow.Amount;
            ExpensesSubmitDate = SelectedRow.SubmitDate;

            object dialogResult = await DialogHost.Show(this, DialogIdentifier);

            if (dialogResult is bool boolResult && boolResult)
            {
                //var expensesToUpdate = ExpensesList.Find(x => x.Id == SelectedRow.Id);
                var expensesToUpdate = ExpensesList.Where(x => x.Id == SelectedRow.Id).SingleOrDefault();

                expensesToUpdate.Title = SelectedExpensesType;
                expensesToUpdate.Amount = ExpensesAmount;
                expensesToUpdate.SubmitDate = ExpensesSubmitDate;

                var result = await CoreGrpcClient.UpdateCategory(expensesToUpdate, expensesToUpdate.Id);

                //if result is empty it means that theres no error.
                if (string.IsNullOrEmpty(result))
                {
                    DisplaySnackbar("Updated.");
                    foreach (var expense in ExpensesList)
                    {
                        if(expense.Id == SelectedRow.Id)
                        {
                            expense.Title = SelectedRow.Title;
                            expense.Amount = SelectedRow.Amount;
                            expense.SubmitDate = SelectedRow.SubmitDate;

                            RaisePropertyChange();
                        }

                        break;
                    }
                }
            }

            //Close?.Invoke(this, EventArgs.Empty);
        }

        private async void DeleteExpenses(object obj)
        {
            if (SelectedRow == null || SelectedRow.Id == null) return;

            var result = await CoreGrpcClient.DeleteCategory(SelectedRow.Id, SelectedRow.Category, LoggedUser.Id);            
            
            if (string.IsNullOrEmpty(result))
            {
                DisplaySnackbar("Removed from your Income.");
                ExpensesList.Remove(SelectedRow);
            }
        }

        private void InitExpensesTypesList()
        {
            ExpensesTypes = new ObservableCollection<string>
            {
                "Home",
                "Family",
                "Transportaion",
                "Shopping",
                "Entertainment",
                "Education",
                "Healthcare",
                "Travel",
                "Personal",
                "Other"
            };
        }

        private void DisplaySnackbar(string content)
        {
            var title = string.IsNullOrEmpty(SelectedExpensesType) ? SelectedRow.Title : SelectedExpensesType;
            IncomeSnackbarMessage = new SnackbarMessage
            {
                ActionContent = "OK",
                ActionCommand = CloseSnackbarCommand,
                Content = $"{title} {content}."
            };

            IsSnackbarActive = true;
        }

        private void CloseSnackbar(object obj)
        {
            IsSnackbarActive = false;
        }

        #endregion Methods
    }
}
