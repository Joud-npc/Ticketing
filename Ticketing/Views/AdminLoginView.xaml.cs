using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ticketing.Commands;
using Ticketing.Data;
using Ticketing.Models;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Views
{
    public partial class AdminLoginView : Window
    {
        private readonly AppDbContext _context;
        public string Email { get; set; }
        
        public RelayCommand InscriptionCommand { get; private set; }

        public AdminLoginView()
        {
            InitializeComponent();
            DataContext = this;
            
            InscriptionCommand = new RelayCommand(_ => OnInscriptionClicked());
            
            InscriptionLink.MouseLeftButtonDown += (s, e) => OnInscriptionClicked();
            
            _context = new AppDbContext();
            
            try
            {
                bool wasCreated = _context.Database.EnsureCreated();
                if (wasCreated)
                {
                    MessageBox.Show("Base de données créée avec succès.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    CreateDefaultAdmin();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de la base de données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void OnInscriptionClicked()
        {
            CreateAccountView createAccountView = new CreateAccountView();
            createAccountView.Show();
            
            this.Close();
        }
        
        private void CreateDefaultAdmin()
        {
            try
            {
                if (_context.Utilisateurs.Any(u => u.Rol == "Admin"))
                    return;
                
                var defaultAdmin = new Utilisateur
                {
                    Nom = "Admin",
                    Prenom = "System",
                    Email = "admin@gryffondor.hp",
                    MDP = "admin123",
                    Rol = "Admin"
                };
                
                _context.Utilisateurs.Add(defaultAdmin);
                _context.SaveChanges();
                
                MessageBox.Show("Compte administrateur par défaut créé.\nEmail: admin@gryffondor.hp\nMot de passe: admin123", 
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

            if (!ValidateHogwartsEmail(email))
            {
                ShowError("L'adresse email doit se terminer par @gryffondor.hp, @poufsouffle.hp, @serdaigle.hp ou @serpentard.hp");
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

        private bool ValidateHogwartsEmail(string email)
        {
            return HogwartsEmailValidationRule.HogwartsHouses
                .Any(house => email.Trim().EndsWith("@" + house, StringComparison.OrdinalIgnoreCase));
        }

        private bool VerifyCredentials(string email, string password)
        {
            try
            {
                var utilisateur = _context.Utilisateurs
                    .ToList()
                    .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.MDP == password);
                
                return utilisateur != null;
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
        
        private void OpenInscriptionView()
        {
            OnInscriptionClicked();
        }
    }
}