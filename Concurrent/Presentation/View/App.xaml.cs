﻿using System.Windows;

namespace Concurrent
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [System.STAThread] // Dodaj atrybut STAThread
                           // Metoda wejścia dla aplikacji
        public static void Main()
        {
            // Inicjalizacja aplikacji
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}