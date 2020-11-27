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
    public class LoansViewModel : BaseViewModel
    {
        private const string DialogIdentifier = "RootDialogHost";
        private const string EditDialogHostIdentifier = "EditDialogHost";

        public LoansViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;
            _loansList = new ObservableCollection<CategoriesModel>();
            LoansSnackbarMessage = new SnackbarMessage();

            if (LoggedUser != null && LoggedUser.LoansList != null)
                InitLoansList(LoggedUser.LoansList);

            InitLoansTypesList();
            LoansSubmitDate = DateTime.Today.Date;
            SelectedMonth = DateTime.Today.Month;
            SelectedYear = DateTime.Today.Year;

            AddLoansCommand = new RelayCommand(AddNewLoans, param => this.CanExecute);
            EditLoansCommand = new RelayCommand(EditLoans, param => this.CanExecute);
            DeleteLoansCommand = new RelayCommand(DeleteLoans, param => this.CanExecute);
            CloseSnackbarCommand = new RelayCommand(CloseSnackbar, param => this.CanExecute);
            FilterListViewCommand = new RelayCommand(OnFilterListView, param => this.CanExecute);
            ClearFilterCommand = new RelayCommand(ClearFilter, param => this.CanExecute);
        }

        #region Properties
        private ObservableCollection<CategoriesModel> _loansList;
        public ObservableCollection<CategoriesModel> LoansList
        {
            get { return _loansList; }
            set
            {
                if (value != _loansList)
                {
                    _loansList = value;
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

        private ObservableCollection<string> _loansTypes;
        public ObservableCollection<string> LoansTypes
        {
            get { return _loansTypes; }
            set
            {
                if (value != _loansTypes)
                {
                    _loansTypes = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _selectedLoansType;
        public string SelectedLoansType
        {
            get { return _selectedLoansType; }
            set
            {
                if (value != _selectedLoansType)
                {
                    _selectedLoansType = value;
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
        public DateTime LoansSubmitDate
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
        public double LoansAmount
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

        private SnackbarMessage _loansSnackbarMessage;
        public SnackbarMessage LoansSnackbarMessage
        {
            get { return _loansSnackbarMessage; }
            set
            {
                if (value != _loansSnackbarMessage)
                {
                    _loansSnackbarMessage = value;
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

        private string _selectedLoanTypeFilter;
        public string SelectedLoanTypeFilter
        {
            get { return _selectedLoanTypeFilter; }
            set
            {
                if (value != _selectedLoanTypeFilter)
                {
                    _selectedLoanTypeFilter = value;
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
        public ICommand AddLoansCommand { get; set; }
        public ICommand EditLoansCommand { get; set; }
        public ICommand DeleteLoansCommand { get; set; }
        public ICommand CloseSnackbarCommand { get; set; }
        public ICommand FilterListViewCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }
        #endregion Commands

        #region Methods

        private void InitLoansList(List<CategoriesModel> loansList)
        {
            foreach (var loan in loansList)
            {
                LoansList.Add(loan);
            }
        }

        private async void AddNewLoans(object obj)
        {
            object dialogResult = await DialogHost.Show(this, DialogIdentifier);

            if (dialogResult is bool boolResult && boolResult)
            {
                var newCategory = new CategoriesModel
                {
                    Category = "Loans",
                    Title = SelectedLoansType,
                    Amount = LoansAmount,
                    SubmitDate = LoansSubmitDate
                };

                LoansList.Add(newCategory);

                var result = await CoreGrpcClient.AddCategory(newCategory, LoggedUser.Id);

                //if result is empty it means that theres no error.
                if (string.IsNullOrEmpty(result))
                {
                    DisplaySnackbar("Added to your Loans");
                }
            }
        }

        private async void EditLoans(object obj)
        {
            if (SelectedRow == null) return;

            SelectedLoansType = SelectedRow.Title;
            LoansAmount = SelectedRow.Amount;
            LoansSubmitDate = SelectedRow.SubmitDate;

            object dialogResult = await DialogHost.Show(this, EditDialogHostIdentifier);

            if (dialogResult is bool boolResult && boolResult)
            {
                var loansToUpdate = LoansList.Where(x => x.Id == SelectedRow.Id).SingleOrDefault();

                loansToUpdate.Title = SelectedLoansType;
                loansToUpdate.Amount = LoansAmount;
                loansToUpdate.SubmitDate = LoansSubmitDate;

                var result = await CoreGrpcClient.UpdateCategory(loansToUpdate, loansToUpdate.Id);

                //if result is empty it means that theres no error.
                if (string.IsNullOrEmpty(result))
                {
                    DisplaySnackbar("Updated.");
                    var tempList = new ObservableCollection<CategoriesModel>();

                    foreach (var loan in LoansList)
                    {
                        if (loan.Id == SelectedRow.Id)
                        {
                            loan.Title = SelectedRow.Title;
                            loan.Amount = SelectedRow.Amount;
                            loan.SubmitDate = SelectedRow.SubmitDate;
                        }
                        tempList = LoansList;
                        break;
                    }
                    LoansList = new ObservableCollection<CategoriesModel>(tempList);
                    ClearSelectedProperties();
                }
            }

            //Close?.Invoke(this, EventArgs.Empty);
        }

        private async void DeleteLoans(object obj)
        {
            if (SelectedRow == null || SelectedRow.Id == null) return;

            var result = await CoreGrpcClient.DeleteCategory(SelectedRow.Id, SelectedRow.Category, LoggedUser.Id);

            if (string.IsNullOrEmpty(result))
            {
                DisplaySnackbar("Removed from your Income.");
                LoansList.Remove(SelectedRow);
            }
        }

        private void InitLoansTypesList()
        {
            LoansTypes = new ObservableCollection<string>
            {
                "",
                "Short Term",
                "Business Loan",
                "Home Refinancing",
                "New Vehicle",
                "Other"
            };
        }

        private void DisplaySnackbar(string content)
        {
            var title = string.IsNullOrEmpty(SelectedLoansType) ? SelectedRow.Title : SelectedLoansType;
            LoansSnackbarMessage = new SnackbarMessage
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
            LoansList = FilterListView("LoansList", SelectedYear, SelectedMonth, SelectedLoanTypeFilter, LoggedUser);
        }

        private void ClearFilter(object obj)
        {
            LoansList = new ObservableCollection<CategoriesModel>(LoggedUser.LoansList);
        }

        private void ClearSelectedProperties()
        {

            SelectedLoansType = string.Empty;
            LoansAmount = 0;
            LoansSubmitDate = DateTime.Today.Date.Date;
        }
        #endregion Methods
    }
}
