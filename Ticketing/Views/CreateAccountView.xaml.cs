using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ticketing.Data;
using Ticketing.Models;

namespace Ticketing.Views
{
    public partial class CreateAccountView : Window
    {
        private readonly AppDbContext _context;
        public string Email { get; set; }

        public CreateAccountView()
        {
            InitializeComponent();
            _context = new AppDbContext();
            DataContext = this;
        }

        private void OnCreateAccountClicked(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            // Validation des champs
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ShowError("Tous les champs sont obligatoires.");
                return;
            }

            if (!ValidateEmail(email))
            {
                ShowError("L'adresse email doit se terminer par @gmail.com.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Les mots de passe ne correspondent pas.");
                return;
            }

            try
            {
                // Vérifier si l'utilisateur existe déjà
                bool exists = _context.Utilisateur.Any(u => u.Email == email);
                if (exists)
                {
                    ShowError("Un compte avec cet email existe déjà.");
                    return;
                }

                // Créer le nouvel utilisateur
                var newUser = new Utilisateur
                {
                    Email = email,
                    MDP = password,
                    Rol = role
                };

                // Enregistrer dans la BDD
                _context.Utilisateur.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show("Compte créé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Retourner à la page de connexion
                var loginView = new AdminLoginView();
                loginView.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du compte : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateEmail(string email)
        {
            return email.Trim().EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase);
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            var loginView = new AdminLoginView();
            loginView.Show();
            this.Close();
        }
    }
}