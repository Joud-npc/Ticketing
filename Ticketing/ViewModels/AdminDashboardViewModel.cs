using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Ticketing.Commands;
using Ticketing.Data;
using Ticketing.Models;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.ViewModels
{
    public class AdminDashboardViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private ObservableCollection<Ticket> _tickets;
        private Ticket _selectedTicket;
        private string _searchText;
        private string _selectedStatus;
        private string _newStatus;
        private bool _logoutRequested;

        public ObservableCollection<Ticket> Tickets
        {
            get => _tickets;
            set
            {
                _tickets = value;
                OnPropertyChanged();
            }
        }

        public Ticket SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                _selectedTicket = value;
                OnPropertyChanged();
                NewStatus = value?.Statut;
                OnPropertyChanged(nameof(IsTicketSelected));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
                LoadTickets();
            }
        }

        public string NewStatus
        {
            get => _newStatus;
            set
            {
                _newStatus = value;
                OnPropertyChanged();
            }
        }

        public bool LogoutRequested
        {
            get => _logoutRequested;
            set
            {
                _logoutRequested = value;
                OnPropertyChanged();
            }
        }

        public bool IsTicketSelected => SelectedTicket != null;

        public ObservableCollection<string> Statuts { get; } = new ObservableCollection<string>
        {
            "Tous les statuts", "Ouvert", "En cours", "Résolu", "Fermé"
        };

        public ICommand RefreshCommand { get; }
        public ICommand UpdateStatusCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminDashboardViewModel()
        {
            _context = new AppDbContext();
            
            try
            {
                _context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur d'initialisation de la base de données: {ex.Message}", 
                    "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            
            Tickets = new ObservableCollection<Ticket>();
            SelectedStatus = "Tous les statuts";
            
            RefreshCommand = new RelayCommand(_ => LoadTickets());
            UpdateStatusCommand = new RelayCommand(_ => UpdateTicketStatus(), _ => SelectedTicket != null);
            SearchCommand = new RelayCommand(_ => LoadTickets());
            LogoutCommand = new RelayCommand(_ => Logout());
            
            LoadTickets();
        }

        private void LoadTickets()
        {
            try
            {
                var query = _context.Tickets.AsQueryable();

                if (SelectedStatus != null && SelectedStatus != "Tous les statuts")
                {
                    query = query.Where(t => t.Statut == SelectedStatus);
                }

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    string searchLower = SearchText.ToLower();
                    query = query.Where(t => 
                        t.Titre.ToLower().Contains(searchLower) || 
                        t.Nom.ToLower().Contains(searchLower) || 
                        t.Email.ToLower().Contains(searchLower)
                    );
                }

                var ticketList = query.OrderByDescending(t => t.DateCreation).ToList();
                
                Tickets.Clear();
                foreach (var ticket in ticketList)
                {
                    Tickets.Add(ticket);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des tickets : {ex.Message}", 
                    "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void UpdateTicketStatus()
        {
            if (SelectedTicket == null || string.IsNullOrWhiteSpace(NewStatus))
                return;

            try
            {
                var ticket = _context.Tickets.Find(SelectedTicket.Id_Ticket);
                if (ticket != null)
                {
                    ticket.Statut = NewStatus;
                    _context.SaveChanges();
                    
                    SelectedTicket.Statut = NewStatus;
                    System.Windows.MessageBox.Show("Statut du ticket mis à jour avec succès.", 
                        "Succès", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de la mise à jour du statut : {ex.Message}", 
                    "Erreur", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void Logout()
        {
            LogoutRequested = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}