using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ticketing.Data;
using Ticketing.Models;
using Microsoft.EntityFrameworkCore;

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
            string nom = NomTextBox.Text;
            string prenom = PrenomTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (RoleComboBox.SelectedItem == null)
            {
                ShowError("Veuillez sélectionner un rôle.");
                return;
            }

            string role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
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
                bool exists = _context.Utilisateurs.Any(u => u.Email == email);
                if (exists)
                {
                    ShowError("Un compte avec cet email existe déjà.");
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
                _context.SaveChanges();
                
                MessageBox.Show("Compte créé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                RetourVersLogin();
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

        private void RetourVersLogin()
        {
            var loginWindow = new AdminLoginView();
            loginWindow.Show();
            this.Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            RetourVersLogin();
        }
    }
}