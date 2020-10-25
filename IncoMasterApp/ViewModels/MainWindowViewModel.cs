using GrpcService.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace IncoMasterApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            switchToHomeViewCommand = new RelayCommand(SwitchToHomeView, param => this.CanExecute);
            switchToIncomeViewCommand = new RelayCommand(SwitchToIncomeView, param => this.CanExecute);
            switchToExpensesViewCommand = new RelayCommand(SwitchToExpensesView, param => this.CanExecute);
            switchToSavingsViewCommand = new RelayCommand(SwitchToSavingsView, param => this.CanExecute);
            switchToLoansViewCommand = new RelayCommand(SwitchToLoansView, param => this.CanExecute);
        }

        #region Commands
        public ICommand switchToHomeViewCommand { get; set; }
        public ICommand switchToIncomeViewCommand { get; set; }
        public ICommand switchToExpensesViewCommand { get; set; }
        public ICommand switchToSavingsViewCommand { get; set; }
        public ICommand switchToLoansViewCommand { get; set; }
        public ICommand itemClickedCommand { get; set; }
        #endregion Commands

        #region Properties
        private object _selectedViewModel;

        public object SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }

        private User _loggedUser;

        public User LoggedUser
        {
            get { return _loggedUser; }
            set { _loggedUser = value; }
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

        #endregion Properties

        #region Methods

        private void SwitchToHomeView(object obj)
        {
            SelectedViewModel = new HomeViewModel();
        }

        private void SwitchToIncomeView(object obj)
        {
            SelectedViewModel = new IncomeViewModel();
        }
        private void SwitchToExpensesView(object obj)
        {
            SelectedViewModel = new ExpensesViewModel();
        }

        private void SwitchToSavingsView(object obj)
        {
            SelectedViewModel = new SavingsViewModel();
        }

        private void SwitchToLoansView(object obj)
        {
            SelectedViewModel = new LoansViewModel();
        }

        #endregion Methods

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
