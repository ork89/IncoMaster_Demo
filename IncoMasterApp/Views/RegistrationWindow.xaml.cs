using IncoMasterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace IncoMasterApp.Views
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            DataContext = new RegistrationViewModel(new WindowService());
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.DataContext = new LoginViewModel(new WindowService());
        }

        private void passwordRegPb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                (DataContext as RegistrationViewModel).SetPassword((sender as PasswordBox).SecurePassword);
        }

        private void ConfPasswordRegPb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                (DataContext as RegistrationViewModel).SetConfPassword((sender as PasswordBox).SecurePassword);
        }
    }
}
