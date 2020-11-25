using DotNetCoreGrpcClient;
using MaterialDesignThemes.Wpf;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IncoMasterApp.ViewModels
{
    public class SavingsViewModel : BaseViewModel
    {
        private const string RootDialogIdentifier = "RootDialogHost";
        private const string EditDialogHostIdentifier = "EditDialogHost";

        public SavingsViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;
            _savingsList = new ObservableCollection<CategoriesModel>();
            SavingsSnackbarMessage = new SnackbarMessage();

            if (LoggedUser != null && LoggedUser.SavingsList != null)
                InitSavingsList(LoggedUser.SavingsList);

            InitSavingsTypesList();
            SavingsSubmitDate = DateTime.Today.Date;
            SelectedMonth = DateTime.Today.Month;
            SelectedYear = DateTime.Today.Year;

            AddSavingsCommand = new RelayCommand(AddNewSavings, param => this.CanExecute);
            EditSavingsCommand = new RelayCommand(EditSavings, param => this.CanExecute);
            DeleteSavingsCommand = new RelayCommand(DeleteSavings, param => this.CanExecute);
            CloseSnackbarCommand = new RelayCommand(CloseSnackbar, param => this.CanExecute);
            FilterListViewCommand = new RelayCommand(OnFilterListView, param => this.CanExecute);
            ClearFilterCommand = new RelayCommand(ClearFilter, param => this.CanExecute);
        }

        #region Properties
        private ObservableCollection<CategoriesModel> _savingsList;
        public ObservableCollection<CategoriesModel> SavingsList
        {
            get { return _savingsList; }
            set
            {
                if (value != _savingsList)
                {
                    _savingsList = value;
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

        private ObservableCollection<string> _savingsTypes;
        public ObservableCollection<string> SavingsTypes
        {
            get { return _savingsTypes; }
            set
            {
                if (value != _savingsTypes)
                {
                    _savingsTypes = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _selectedSavingsType;
        public string SelectedSavingsType
        {
            get { return _selectedSavingsType; }
            set
            {
                if (value != _selectedSavingsType)
                {
                    _selectedSavingsType = value;
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
        public DateTime SavingsSubmitDate
        {
            get { return _submitDate; }
            set
            {
                if (value != _submitDate)
                {
                    _submitDate = value.Date;
                    RaisePropertyChanged();
                }
            }
        }

        private double _amount;
        public double SavingsAmount
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

        private SnackbarMessage _savingsSnackbarMessage;
        public SnackbarMessage SavingsSnackbarMessage
        {
            get { return _savingsSnackbarMessage; }
            set
            {
                if (value != _savingsSnackbarMessage)
                {
                    _savingsSnackbarMessage = value;
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

        private string _selectedSavingsTypeFilter;
        public string SelectedSavingsTypeFilter
        {
            get { return _selectedSavingsTypeFilter; }
            set
            {
                if (value != _selectedSavingsTypeFilter)
                {
                    _selectedSavingsTypeFilter = value;
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
        public ICommand AddSavingsCommand { get; set; }
        public ICommand EditSavingsCommand { get; set; }
        public ICommand DeleteSavingsCommand { get; set; }
        public ICommand CloseSnackbarCommand { get; set; }
        public ICommand FilterListViewCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }
        #endregion Commands

        #region Methods

        private void InitSavingsList(List<CategoriesModel> savingsList)
        {
            foreach (var savings in savingsList)
            {
                SavingsList.Add(savings);
            }
        }

        private async void AddNewSavings(object obj)
        {
            object dialogResult = await DialogHost.Show(this, RootDialogIdentifier);

            if (dialogResult is bool boolResult && boolResult)
            {
                var newCategory = new CategoriesModel
                {
                    Category = "Savings",
                    Title = SelectedSavingsType,
                    Amount = SavingsAmount,
                    SubmitDate = SavingsSubmitDate
                };

                SavingsList.Add(newCategory);

                var result = await CoreGrpcClient.AddCategory(newCategory, LoggedUser.Id);

                //if result is empty it means that theres no error.
                if (string.IsNullOrEmpty(result))
                {
                    DisplaySnackbar("Added to your savings");
                }
            }
        }

        private async void EditSavings(object obj)
        {
            if (SelectedRow == null) return;

            SelectedSavingsType = SelectedRow.Title;
            SavingsAmount = SelectedRow.Amount;
            SavingsSubmitDate = SelectedRow.SubmitDate;

            object dialogResult = await DialogHost.Show(this, EditDialogHostIdentifier);

            if (dialogResult is bool boolResult && boolResult)
            {
                var savingsToUpdate = SavingsList.Where(x => x.Id == SelectedRow.Id).SingleOrDefault();

                savingsToUpdate.Title = SelectedSavingsType;
                savingsToUpdate.Amount = SavingsAmount;
                savingsToUpdate.SubmitDate = SavingsSubmitDate;

                var result = await CoreGrpcClient.UpdateCategory(savingsToUpdate, savingsToUpdate.Id);

                //if result is empty it means that theres no error.
                if (string.IsNullOrEmpty(result))
                {
                    DisplaySnackbar("Updated.");
                    
                    foreach (var savings in SavingsList)
                    {
                        if (savings.Id == SelectedRow.Id)
                        {
                            savings.Title = SelectedRow.Title;
                            savings.Amount = SelectedRow.Amount;
                            savings.SubmitDate = SelectedRow.SubmitDate;

                            RaisePropertyChanged();
                        }

                        break;
                    }
                }
            }

            //Close?.Invoke(this, EventArgs.Empty);
        }

        private async void DeleteSavings(object obj)
        {
            if (SelectedRow == null || SelectedRow.Id == null) return;

            var result = await CoreGrpcClient.DeleteCategory(SelectedRow.Id, SelectedRow.Category, LoggedUser.Id);

            if (string.IsNullOrEmpty(result))
            {
                DisplaySnackbar("Removed from your Income.");
                SavingsList.Remove(SelectedRow);
            }
        }

        private void InitSavingsTypesList()
        {
            SavingsTypes = new ObservableCollection<string>
            {
                "",
                "Savings Account",
                "High-Yield Savings Account",
                "Stocks and Bonds",
                "Certificate of Deposit",
                "Other"
            };
        }

        private void DisplaySnackbar(string content)
        {
            var title = string.IsNullOrEmpty(SelectedSavingsType) ? SelectedRow.Title : SelectedSavingsType;
            SavingsSnackbarMessage = new SnackbarMessage
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

        private void OnFilterListView(object obj)
        {
            SavingsList = FilterListView("SavingsList", SelectedYear, SelectedMonth, SelectedSavingsTypeFilter, LoggedUser);
        }

        private void ClearFilter(object obj)
        {
            SavingsList = new ObservableCollection<CategoriesModel>(LoggedUser.SavingsList);
        }
        #endregion Methods
    }
}
