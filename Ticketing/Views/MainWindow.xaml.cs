using System.Windows;
using Ticketing.ViewModels;


namespace Ticketing.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}