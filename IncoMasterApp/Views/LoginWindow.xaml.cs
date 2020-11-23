using IncoMasterApp.Interfaces;
using IncoMasterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace IncoMasterApp.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel(new WindowService());
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs args)
        {
            //DragMove();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            //this.DataContext = new MainWindowViewModel();
            Application.Current.Shutdown();
        }

        private void passwordPb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                (DataContext as LoginViewModel).SetPassword((sender as PasswordBox).SecurePassword);
        }
    }
}
