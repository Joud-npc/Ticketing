using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
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

        public class PrioriteToColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is string priorite)
                {
                    return priorite.ToLower() switch
                    {
                        "haute" => Brushes.Red,
                        "moyenne" => Brushes.Orange,
                        "basse" => Brushes.Green,
                        _ => Brushes.Gray
                    };
                }

                return Brushes.Gray;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}