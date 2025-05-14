using System.Windows;

namespace Ticketing.Views
{
    public partial class TicketView : Window
    {
        public TicketView()
        {
            InitializeComponent();
            DataContext = new Ticketing.ViewModels.TicketViewModel();
        }
        
        private void OnRetourClicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainView();
            mainWindow.Show();
        
            this.Close();
        }
    }
}