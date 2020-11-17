using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IncoMasterApp.ViewModels
{
    public class IncomeViewModel : BaseViewModel
    {
        public IncomeViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;

            if (LoggedUser != null)
                IncomeList = LoggedUser.IncomeList;

            AddIncomeCommand = new RelayCommand(AddNewIncome, param => this.CanExecute);
            EditIncomeCommand = new RelayCommand(EditIncome, param => this.CanExecute);
            DeleteIncomeCommand = new RelayCommand(DeleteIncome, param => this.CanExecute);
        }

        #region Properties
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

        #region Commands
        public  ICommand AddIncomeCommand { get; set; }
        public  ICommand EditIncomeCommand { get; set; }
        public  ICommand DeleteIncomeCommand { get; set; }
        #endregion

        #region Methods
        private void DeleteIncome(object obj)
        {
            throw new NotImplementedException();
        }

        private void EditIncome(object obj)
        {
            throw new NotImplementedException();
        }

        private void AddNewIncome(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
