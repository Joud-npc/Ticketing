using System.Windows;
using Ticketing.ViewModels;
using Ticketing.Views;

namespace Ticketing.Views
{
    /// <summary>
    /// Interaction logic for AdminDashboardView.xaml
    /// </summary>
    public partial class AdminDashboardView : Window
    {
        private readonly AdminDashboardViewModel _viewModel;
        
        public AdminDashboardView()
        {
            InitializeComponent();
            _viewModel = DataContext as AdminDashboardViewModel ?? new AdminDashboardViewModel();
            
            _viewModel.PropertyChanged += (sender, args) => {
                if (args.PropertyName == "LogoutRequested" && _viewModel.LogoutRequested)
                {
                    Logout();
                }
            };
        }
        
        private void Logout()
        {
            var loginView = new AdminLoginView();
            loginView.Show();
            this.Close();
        }
    }
}