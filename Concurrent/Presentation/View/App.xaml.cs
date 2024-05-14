using System.Configuration;
using System.Data;
using System.Windows;

namespace Concurrent;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [STAThread] // Dodaj atrybut STAThread
                // Metoda wejścia dla aplikacji
    public static void Main()
    {
        // Inicjalizacja aplikacji
        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}