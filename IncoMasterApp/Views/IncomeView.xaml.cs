using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IncoMasterApp.Views
{
    /// <summary>
    /// Interaction logic for IncomeControl.xaml
    /// </summary>
    public partial class IncomeView : UserControl
    {
        public IncomeView()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((sender as TextBox).Text == "0")
                (sender as TextBox).Text = "";
        }
    }
}
