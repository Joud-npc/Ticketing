using System;
using System.Linq;
using System.Windows;
using Ticketing.Data;
using Ticketing.Models;

namespace Ticketing.Views
{
    public partial class AdminLoginView : Window
    {
        private readonly AppDbContext _context;
        public string Email { get; set; }

        public AdminLoginView()
        {
            InitializeComponent();
            _context = new AppDbContext();
            DataContext = this;
        }

        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Tous les champs sont obligatoires.");
                return;
            }

            if (!ValidateEmail(email))
            {
                ShowError("L'adresse email doit se terminer par @gmail.com.");
                return;
            }

            if (VerifyCredentials(email, password))
            {
                ErrorMessage.Visibility = Visibility.Collapsed;
                OpenAdminDashboard();
            }
            else
            {
                ShowError("Email ou mot de passe incorrect.");
            }
        }

        private bool ValidateEmail(string email)
        {
            return email.Trim().EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase);
        }

        private bool VerifyCredentials(string email, string password)
        {
            try
            {
                return _context.Utilisateur
                    .Any(u => u.Email == email && u.MDP == password && u.Rol == "Admin");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur BDD : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void OnOpenCreateAccountClicked(object sender, RoutedEventArgs e)
        {
            var createAccountWindow = new CreateAccountView();
            createAccountWindow.Show();
            this.Close();
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private void OpenAdminDashboard()
        {
            var dashboard = new AdminDashboardView();
            dashboard.Show();
            this.Close();
        }

        private void OnRetourClicked(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainView();
            mainWindow.Show();
            this.Close();
        }
    }
}