using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ticketing.Data;
using Ticketing.Models;
using Microsoft.EntityFrameworkCore;

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
            DataContext = this;
            
            _context = new AppDbContext();
            
            try
            {
                // Assurez-vous que la base de données est créée
                bool wasCreated = _context.Database.EnsureCreated();
                if (wasCreated)
                {
                    MessageBox.Show("Base de données créée avec succès.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Créer un compte admin par défaut si la BDD vient d'être créée
                    CreateDefaultAdmin();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de la base de données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void CreateDefaultAdmin()
        {
            try
            {
                // Vérifier si un admin existe déjà
                if (_context.Utilisateurs.Any(u => u.Rol == "Admin"))
                    return;
                
                // Créer un compte admin par défaut
                var defaultAdmin = new Utilisateur
                {
                    Nom = "Admin",
                    Prenom = "System",
                    Email = "admin@gmail.com",
                    MDP = "admin123",
                    Rol = "Admin"
                };
                
                _context.Utilisateurs.Add(defaultAdmin);
                _context.SaveChanges();
                
                MessageBox.Show("Compte administrateur par défaut créé.\nEmail: admin@gmail.com\nMot de passe: admin123", 
                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du compte admin par défaut: {ex.Message}", 
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                // Solution 1: Utiliser ToList() pour exécuter la requête côté client
                var utilisateur = _context.Utilisateurs
                    .ToList()
                    .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.MDP == password);
                
                return utilisateur != null;
                
                /* Solution 2: Réécrire la requête pour qu'elle soit compatible avec EF Core
                return _context.Utilisateurs
                    .Where(u => u.Email.ToLower() == email.ToLower() && u.MDP == password)
                    .Any();
                */
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
                // Également mis à jour cette requête pour éviter le même problème
                bool exists = _context.Utilisateurs.ToList().Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
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