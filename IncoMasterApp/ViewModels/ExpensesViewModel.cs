using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IncoMasterApp.ViewModels
{
    public class ExpensesViewModel : BaseViewModel
    {
        public ExpensesViewModel()
        {
            LoggedUser = MainWindowViewModel.Instance.LoggedUser;

            if (LoggedUser != null)
                ExpensesList = LoggedUser.ExpensesList;
        }

        #region Properties
        private IList<CategoriesModel> _expensesList;
        public IList<CategoriesModel> ExpensesList
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
        #endregion
    }
}
