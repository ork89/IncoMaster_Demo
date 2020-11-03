using IncoMasterApp.ViewModels;
using System.Windows;
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

            LoginViewModel vm = new LoginViewModel();
            this.DataContext = vm;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs args)
        {
            DragMove();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            this.Close();
            this.DataContext = MainWindowViewModel.Instance;
        }
    }
}
