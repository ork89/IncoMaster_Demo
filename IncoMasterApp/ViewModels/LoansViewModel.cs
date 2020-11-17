using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IncoMasterApp.ViewModels
{
    public class LoansViewModel : BaseViewModel
    {
        public LoansViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;

            if (LoggedUser != null)
                LoansList = LoggedUser.LoansList;
        }

        #region Properties
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
        #endregion
    }
}
