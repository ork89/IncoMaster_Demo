using IncoMasterApp.Interfaces;
using IncoMasterApp.ViewModels;
using IncoMasterApp.Views;
using System.Windows;

namespace IncoMasterApp
{
    public class WindowService : IWindowService
    {
        public void OpenRegistration(RegistrationViewModel vm)
        {
            RegistrationWindow reg = new RegistrationWindow();

            reg.DataContext = vm;
            reg.ShowDialog();
        }

        public void OpenMainWindow(MainWindowViewModel mvm)
        {
            MainWindow mWin = new MainWindow();

            mWin.DataContext = mvm;
            mWin.ShowDialog();
        }

        public void OpenLogin(LoginViewModel lvm)
        {
            MainWindow mainWindow = new MainWindow();

            mainWindow.DataContext = new MainWindowViewModel(new WindowService());
            mainWindow.Hide();

            LoginWindow window = new LoginWindow();

            window.DataContext = lvm;
            window.Activate();
            window.ShowDialog();
        }
    }
}
