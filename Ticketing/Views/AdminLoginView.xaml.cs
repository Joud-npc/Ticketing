using System;
using System.Linq;
using System.Windows;
using Ticketing.Data; // Namespace de ApplicationDbContext
using Ticketing.Models; // Namespace de ton modèle Utilisateur

namespace Ticketing.Views
{
    public partial class AdminLoginView : Window
    {
        private readonly AppDbContext _context;

        public AdminLoginView()
        {
            InitializeComponent();
            _context = new AppDbContext(); // à remplacer par une injection si besoin
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

            if (VerifyCredentials(email, password))
            {
                ErrorMessage.Visibility = Visibility.Collapsed;
                OpenAdminDashboard();
            }
            else
            {
                ShowError("Compte non trouvé. Vous pouvez en créer un.");
                CreateAccountButton.Visibility = Visibility.Visible;
            }
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

        private void OnCreateAccountClicked(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Veuillez remplir les champs.");
                return;
            }

            try
            {
                bool exists = _context.Utilisateur.Any(u => u.Email == email);
                if (exists)
                {
                    ShowError("Ce compte existe déjà.");
                    return;
                }

                var newUser = new Utilisateur
                {
                    Email = email,
                    MDP = password,
                    Rol = "Admin"
                };

                _context.Utilisateur.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show("Compte créé avec succès !");
                OpenAdminDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur création de compte : {ex.Message}");
            }
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
    }
}
