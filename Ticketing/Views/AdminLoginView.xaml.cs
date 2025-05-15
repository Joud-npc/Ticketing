// Modification d'AdminLoginView.xaml.cs pour assurer la création de la base de données
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ticketing.Data;
using Ticketing.Models;
using Microsoft.EntityFrameworkCore;
using YourProject.Views;

namespace Ticketing.Views
{
    public partial class AdminLoginView : Window
    {
        private readonly AppDbContext _context;
        public string Email { get; set; }
        public string NewEmail { get; set; }

        public AdminLoginView()
        {
            InitializeComponent();
            _context = new AppDbContext();
            
            try
            {
                // Assurez-vous que la base de données est créée
                bool wasCreated = _context.Database.EnsureCreated();
                if (wasCreated)
                {
                    MessageBox.Show("Base de données créée avec succès.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
                // Vérifiez si les migrations sont à jour
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de la base de données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
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
                // Ici nous utilisons le DbSet Utilisateurs (au lieu de Utilisateur)
                return _context.Utilisateurs
                    .Any(u => u.Email == email && u.MDP == password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur BDD : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
        
        private void ShowCreateError(string message)
        {
            CreateErrorMessage.Text = message;
            CreateErrorMessage.Visibility = Visibility.Visible;
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
        
        private void OnCreateAccountClicked(object sender, RoutedEventArgs e)
        {
            string nom = CreateNomTextBox.Text;
            string prenom = CreatePrenomTextBox.Text;
            string email = CreateEmailTextBox.Text;
            string password = CreatePasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (RoleComboBox.SelectedItem == null)
            {
                ShowCreateError("Veuillez sélectionner un rôle.");
                return;
            }

            string role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ShowCreateError("Tous les champs sont obligatoires.");
                return;
            }

            if (!ValidateEmail(email))
            {
                ShowCreateError("L'adresse email doit se terminer par @gmail.com.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowCreateError("Les mots de passe ne correspondent pas.");
                return;
            }

            try
            {
                // Ici nous utilisons le DbSet Utilisateurs (au lieu de Utilisateur)
                bool exists = _context.Utilisateurs.Any(u => u.Email == email);
                if (exists)
                {
                    ShowCreateError("Un compte avec cet email existe déjà.");
                    return;
                }

                var newUser = new Utilisateur
                {
                    Nom = nom,
                    Prenom = prenom,
                    Email = email,
                    MDP = password,
                    Rol = role
                };

                // Ici nous utilisons le DbSet Utilisateurs (au lieu de Utilisateur)
                _context.Utilisateurs.Add(newUser);
                try
                {
                    _context.SaveChanges();
                    MessageBox.Show("Compte créé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    OpenAdminDashboard();
                }
                catch (DbUpdateException dbEx)
                {
                    var innerEx = dbEx.InnerException?.Message ?? dbEx.Message;
                    MessageBox.Show($"Erreur de base de données : {innerEx}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du compte : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}