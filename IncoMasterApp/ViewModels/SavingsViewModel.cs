using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IncoMasterApp.ViewModels
{
    public class SavingsViewModel : BaseViewModel
    {
        public SavingsViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;

            if (LoggedUser != null)
                SavingsList = LoggedUser.SavingsList;
        }

        #region Properties
        private IList<CategoriesModel> _savingsList;
        public IList<CategoriesModel> SavingsList
        {
            get { return _savingsList; }
            set
            {
                if (value != _savingsList)
                {
                    _savingsList = value;
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
