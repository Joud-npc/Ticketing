using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Ticketing.Commands;
using Ticketing.Data;
using Ticketing.Models;

namespace Ticketing.ViewModels
{
    public class TicketViewModel : INotifyPropertyChanged
    {
        private string titre, description, nom, prenom, email, categorie;
        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>
        {
            "Support Technique", "Facturation", "Demande générale", "Bug"
        };

        public string Titre { get => titre; set { titre = value; OnPropertyChanged(); } }
        public string Description { get => description; set { description = value; OnPropertyChanged(); } }
        public string Nom { get => nom; set { nom = value; OnPropertyChanged(); } }
        public string Prenom { get => prenom; set { prenom = value; OnPropertyChanged(); } }
        public string Email { get => email; set { email = value; OnPropertyChanged(); } }
        public string Categorie { get => categorie; set { categorie = value; OnPropertyChanged(); } }

        public ICommand EnvoyerCommand { get; }

        public TicketViewModel()
        {
            EnvoyerCommand = new RelayCommand(_ => EnvoyerTicket());
        }

        private void EnvoyerTicket()
        {
            if (string.IsNullOrWhiteSpace(Titre) ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(Nom) ||
                string.IsNullOrWhiteSpace(Prenom) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Categorie))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Champs manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var trimmedEmail = Email.Trim();
            if (!trimmedEmail.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("L'adresse email doit se terminer par @gmail.com.", "Email invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                using var db = new AppDbContext();
                var nouveauTicket = new Ticket
                {
                    Titre = Titre,
                    Descriptions = Description,
                    Nom = Nom,
                    Prenom = Prenom,
                    Email = trimmedEmail,
                    Categorie = Categorie,
                    DateCreation = DateTime.Now
                };

                db.Tickets.Add(nouveauTicket);
                db.SaveChanges();
                MessageBox.Show("Ticket créé avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            Titre = Description = Nom = Prenom = Email = Categorie = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}