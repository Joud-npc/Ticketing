using System;
using System.Windows;
using System.Windows.Controls;
using Ticketing.Data;
using Ticketing.Models;
using System.Linq;

namespace Ticketing.Views
{
    public partial class CreateAccountView : Window
    {
        private readonly AppDbContext _context;
        public string Email { get; set; }

        public CreateAccountView()
        {
            InitializeComponent();
            DataContext = this;
            
            _context = new AppDbContext();
        }

        private void OnCreateAccountClicked(object sender, RoutedEventArgs e)
        {
            string nom = NomTextBox.Text;
            string prenom = PrenomTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Tous les champs sont obligatoires.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Les mots de passe ne correspondent pas.");
                return;
            }

            if (!ValidateHogwartsEmail(email))
            {
                ShowError("L'adresse email doit se terminer par @gryffondor.hp, @poufsouffle.hp, @serdaigle.hp ou @serpentard.hp");
                return;
            }

            if (IsEmailAlreadyUsed(email))
            {
                ShowError("Cette adresse email est déjà utilisée.");
                return;
            }

            try
            {
                var newUser = new Utilisateur
                {
                    Nom = nom,
                    Prenom = prenom,
                    Email = email,
                    MDP = password,
                    Rol = role
                };

                _context.Utilisateurs.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show($"Compte créé avec succès pour {prenom} {nom}!", 
                    "Création réussie", MessageBoxButton.OK, MessageBoxImage.Information);

                var loginView = new AdminLoginView();
                loginView.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création du compte : {ex.Message}", 
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateHogwartsEmail(string email)
        {
            return Commands.HogwartsEmailValidationRule.HogwartsHouses
                .Any(house => email.Trim().EndsWith("@" + house, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsEmailAlreadyUsed(string email)
        {
            try
            {
                return _context.Utilisateurs
                    .Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                return false;
            }
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