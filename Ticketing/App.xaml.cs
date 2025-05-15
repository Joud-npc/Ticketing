// Modification du fichier App.xaml.cs pour initialiser la base de données au démarrage
using System.Globalization;
using System.Threading;
using System.Windows;
using Ticketing.Data;
using Microsoft.EntityFrameworkCore;

namespace Ticketing;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Définir la culture sur invariant ou en-US pour éviter le chargement de ressources françaises
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        
        // Initialisation de la base de données au démarrage
        InitializeDatabase();
        
        base.OnStartup(e);
    }
    
    private void InitializeDatabase()
    {
        try
        {
            using var context = new AppDbContext();
            
            // Vérifier si la base de données existe, sinon la créer
            context.Database.EnsureCreated();
            
            // Appliquer les migrations en attente si nécessaire
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show($"Erreur d'initialisation de la base de données : {ex.Message}", 
                "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}