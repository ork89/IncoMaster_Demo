using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IncoMasterApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            InitMonthsAndYearsLists();
        }

        public List<int> MonthsList { get; set; }
        public List<int> YearsList { get; set; }
        private void InitMonthsAndYearsLists()
        {
            MonthsList = new List<int>();
            YearsList = new List<int>();

            for (int m = 1; m < 13; m++)
            {
                MonthsList.Add(m);
            }

            var currentYear = DateTime.Today.Year;

            for (int y = currentYear - 5; y < currentYear + 4; y++)
            {
                YearsList.Add(y);
            }
        }

        public ObservableCollection<CategoriesModel> FilterListView(string categoryType,int year, int month, string title, UserModel user)
        {
            var categoryList = new List<CategoriesModel>();
            switch (categoryType)
            {
                case "IncomeList":
                    categoryList = user.IncomeList;
                    break;
                case "ExpensesList":
                    categoryList = user.ExpensesList;
                    break;
                case "SavingsList":
                    categoryList = user.SavingsList;
                    break;
                case "LoansList":
                    categoryList = user.LoansList;
                    break;
                default:
                    break;
            }
            var tempList = new List<CategoriesModel>();

            if (string.IsNullOrEmpty(title))
            {
                tempList = categoryList.Select(x => x).Where(s => s.SubmitDate.Year == year && s.SubmitDate.Month == month).ToList();
            }
            else
                tempList = categoryList.Select(x => x).Where(x => x.Title == title && x.SubmitDate.Year == year && x.SubmitDate.Month == month).ToList();

            return new ObservableCollection<CategoriesModel>(tempList);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
