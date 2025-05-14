using System.Windows;
using System.Windows.Input;
using Ticketing.Views;
using Ticketing.Commands;

namespace Ticketing.ViewModels
{
    public class MainViewModel
    {
        public ICommand OuvrirTicketCommand { get; }
        public ICommand OpenAdminLoginCommand { get; }

        public MainViewModel()
        {
            OuvrirTicketCommand = new RelayCommand(OuvrirTicket);
            OpenAdminLoginCommand = new RelayCommand(OpenAdminLogin);
        }

        private void OuvrirTicket(object _)
        {
            var ticketWindow = new TicketView();
            ticketWindow.Show();

            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainView mainView)
                {
                    mainView.Close();
                    break;
                }
            }
        }

        private void OpenAdminLogin(object _)
        {
            var loginWindow = new AdminLoginView();
            loginWindow.Show();
            
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainView mainView)
                {
                    mainView.Close();
                    break;
                }
            }
        }
    }
}