using IncoMasterApp.ViewModels;
using IncoMasterApp.Views;
using System.Windows;

namespace IncoMasterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            LoginWindow win = new LoginWindow();
            win.Owner = Owner;
            win.Show();
            win.Activate();
            win.Topmost = true;

            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
