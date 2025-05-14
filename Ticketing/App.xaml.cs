using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows;

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
        
        base.OnStartup(e);
    }
}