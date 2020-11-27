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
    public class IncomeViewModel : BaseViewModel
    {
        private const string DialogIdentifier = "RootDialogHost";
        private const string EditDialogHostIdentifier = "EditDialogHost";

        public IncomeViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;
            IncomeList = new ObservableCollection<CategoriesModel>();
            IncomeSnackbarMessage = new SnackbarMessage();

            if (LoggedUser != null && LoggedUser.IncomeList != null)
                InitIncomeList(LoggedUser.IncomeList);

            InitIncomeTypesList();
            IncomeSubmitDate = DateTime.Today;
            SelectedMonth = DateTime.Today.Month;
            SelectedYear = DateTime.Today.Year;

            AddIncomeCommand = new RelayCommand(AddNewIncome, param => this.CanExecute);
            EditIncomeCommand = new RelayCommand(EditIncome, param => this.CanExecute);
            DeleteIncomeCommand = new RelayCommand(DeleteIncome, param => this.CanExecute);
            CloseSnackbarCommand = new RelayCommand(CloseSnackbar, param => this.CanExecute);
            FilterListViewCommand = new RelayCommand(OnFilterListView, param => this.CanExecute);
            ClearFilterCommand = new RelayCommand(ClearFilter, param => this.CanExecute);
        }

        #region Properties
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

        private ObservableCollection<CategoriesModel> _incomeList;
        public ObservableCollection<CategoriesModel> IncomeList
        {
            get { return _incomeList; }
            set
            {
                if (value != _incomeList)
                {
                    _incomeList = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<string> _incomeTypes;
        public ObservableCollection<string> IncomeTypes
        {
            get { return _incomeTypes; }
            set
            {
                if (value != _incomeTypes)
                {
                    _incomeTypes = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _selectedIncomeType;
        public string SelectedIncomeType
        {
            get { return _selectedIncomeType; }
            set
            {
                if (value != _selectedIncomeType)
                {
                    _selectedIncomeType = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        private DateTime _submitDate;
        public DateTime IncomeSubmitDate
        {
            get { return _submitDate; }
            set
            {
                if (value != _submitDate)
                {
                    _submitDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        private double _amount;
        public double IncomeAmount
        {
            get { return _amount; }
            set
            {
                if (value != _amount)
                {
                    _amount = value;
                    RaisePropertyChanged();
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

        private string _selectedIncomeTypeFilter;
        public string SelectedIncomeTypeFilter
        {
            get { return _selectedIncomeTypeFilter; }
            set
            {
                if (value != _selectedIncomeTypeFilter)
                {
                    _selectedIncomeTypeFilter = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }

        public List<int> Months { get { return base.MonthsList; } }
        public List<int> Years { get { return base.YearsList; } }


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
        public ICommand AddIncomeCommand { get; set; }
        public ICommand EditIncomeCommand { get; set; }
        public ICommand DeleteIncomeCommand { get; set; }
        public ICommand CloseSnackbarCommand { get; set; }
        public ICommand FilterListViewCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }
        #endregion Commands

        #region Methods

        private void InitIncomeList(List<CategoriesModel> incomeList)
        {
            foreach (var income in incomeList)
            {
                IncomeList.Add(income);
            }
        }

        private void InitIncomeTypesList()
        {
            IncomeTypes = new ObservableCollection<string>
            {
                "",
                "Salary",
                "Second Income",
                "One Time Payment",
                "Rent",
                "Scholarship",
                "Other",
            };
        }

        private async void AddNewIncome(object obj)
        {
            object dialogResult = await DialogHost.Show(this, DialogIdentifier);

            if (dialogResult is bool boolResult && boolResult)
            {
                var newCategory = new CategoriesModel
                {
                    Category = "Income",
                    Title = SelectedIncomeType,
                    Amount = IncomeAmount,
                    SubmitDate = IncomeSubmitDate
                };

                IncomeList.Add(newCategory);

                var result = await CoreGrpcClient.AddCategory(newCategory, LoggedUser.Id);

                //if result is empty it means that theres no error.
                if (string.IsNullOrEmpty(result))
                {
                    DisplaySnackbar("Added to your income");
                }
            }
        }

        private async void EditIncome(object obj)
        {
            if (SelectedRow == null) return;

            SelectedIncomeType = SelectedRow.Title;
            IncomeAmount = SelectedRow.Amount;
            IncomeSubmitDate = SelectedRow.SubmitDate;

            var categoryToUpdate = new CategoriesModel();
            object dialogResult = await DialogHost.Show(this, EditDialogHostIdentifier);

            if (dialogResult is bool boolResult && boolResult)
            {
                categoryToUpdate = IncomeList.Where(x => x.Id == SelectedRow.Id).SingleOrDefault();

                categoryToUpdate.Title = SelectedIncomeType;
                categoryToUpdate.Amount = IncomeAmount;
                categoryToUpdate.SubmitDate = IncomeSubmitDate;

                var result = await CoreGrpcClient.UpdateCategory(categoryToUpdate, categoryToUpdate.Id);

                //if result is empty it means that theres no error.
                if (string.IsNullOrEmpty(result))
                {
                    DisplaySnackbar("Updated.");
                    var tempList = new ObservableCollection<CategoriesModel>();
                    foreach (var income in IncomeList)
                    {
                        if (income.Id == SelectedRow.Id)
                        {
                            income.Title = SelectedRow.Title;
                            income.Amount = SelectedRow.Amount;
                            income.SubmitDate = SelectedRow.SubmitDate;
                        }
                        tempList = IncomeList;
                        break;
                    }
                    IncomeList = new ObservableCollection<CategoriesModel>(tempList);
                    ClearSelectedProperties();
                }
            }

            //Close?.Invoke(this, EventArgs.Empty);
        }

        private async void DeleteIncome(object obj)
        {
            if (SelectedRow == null || SelectedRow.Id == null) return;

            var result = await CoreGrpcClient.DeleteCategory(SelectedRow.Id, SelectedRow.Category, LoggedUser.Id);

            if (string.IsNullOrEmpty(result))
            {
                DisplaySnackbar("Removed from your Income.");
                IncomeList.Remove(SelectedRow);
            }
        }

        private void DisplaySnackbar(string content)
        {
            var title = string.IsNullOrEmpty(SelectedIncomeType) ? SelectedRow.Title : SelectedIncomeType;
            IncomeSnackbarMessage = new SnackbarMessage
            {
                ActionContent = "OK",
                ActionCommand = CloseSnackbarCommand,
                Content = $"{title} {content}.",
            };

            IsSnackbarActive = true;
        }

        private void CloseSnackbar(object obj)
        {
            IsSnackbarActive = false;
        }

        private void OnFilterListView(object obj)
        {
            IncomeList = FilterListView("IncomeList", SelectedYear, SelectedMonth, SelectedIncomeTypeFilter, LoggedUser);
        }

        private void ClearFilter(object obj)
        {
            IncomeList = new ObservableCollection<CategoriesModel>(LoggedUser.IncomeList);
        }

        private void ClearSelectedProperties()
        {

            SelectedIncomeType = string.Empty;
            IncomeAmount = 0;
            IncomeSubmitDate = DateTime.Today.Date.Date;
        }
        #endregion Methods
    }
}
