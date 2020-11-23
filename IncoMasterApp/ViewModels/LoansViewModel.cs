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

        public LoansViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;
            _loansList = new ObservableCollection<CategoriesModel>();

            if (LoggedUser != null)
                InitLoansList(LoggedUser.LoansList);

            AddLoansCommand = new RelayCommand(AddNewLoans, param => this.CanExecute);
            EditLoansCommand = new RelayCommand(EditLoans, param => this.CanExecute);
            DeleteLoansCommand = new RelayCommand(DeleteLoans, param => this.CanExecute);
            CloseSnackbarCommand = new RelayCommand(CloseSnackbar, param => this.CanExecute);

            InitLoansTypesList();
            LoansSubmitDate = DateTime.Today.Date;
            IncomeSnackbarMessage = new SnackbarMessage();
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

        private ObservableCollection<string> _loansTypes;
        public ObservableCollection<string> LoansTypes
        {
            get { return _loansTypes; }
            set
            {
                if (value != _loansTypes)
                {
                    _loansTypes = value;
                    RaisePropertyChange();
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
        public DateTime LoansSubmitDate
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
        public double LoansAmount
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
        public ICommand AddLoansCommand { get; set; }
        public ICommand EditLoansCommand { get; set; }
        public ICommand DeleteLoansCommand { get; set; }
        public ICommand CloseSnackbarCommand { get; set; }
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

            object dialogResult = await DialogHost.Show(this, DialogIdentifier);

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

                    foreach (var loan in LoansList)
                    {
                        if (loan.Id == SelectedRow.Id)
                        {
                            loan.Title = SelectedRow.Title;
                            loan.Amount = SelectedRow.Amount;
                            loan.SubmitDate = SelectedRow.SubmitDate;

                            RaisePropertyChange();
                        }

                        break;
                    }
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
