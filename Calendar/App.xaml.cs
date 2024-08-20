using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WeeklyPlanner.Data.Data;
using WeeklyPlanner.Data.Repositories;
using WeeklyPlanner.UI.ViewModels;
using WeeklyPlanner.UI.Views;

namespace Calendar
{
    /// <summary>
    /// Hauptanwendungsklasse, die den Dienstanbieter einrichtet und die Abhängigkeiten registriert.
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// Konstruktor der App-Klasse. 
        /// Initialisiert die Dienstsammlung, konfiguriert die Dienste und baut den Dienstanbieter auf.
        /// </summary>
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Wird beim Start der Anwendung aufgerufen. 
        /// Hier wird das Hauptfenster der Anwendung erstellt und angezeigt.
        /// </summary>
        /// <param name="theArgs">Argumente für das Startereignis.</param>
        protected override void OnStartup(StartupEventArgs theArgs)
        {
            base.OnStartup(theArgs);

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        /// <summary>
        /// Konfiguriert die Dienste, die von der Anwendung benötigt werden. 
        /// Registriert die Datenbankkontext, Repositories, ViewModels und Views.
        /// </summary>
        /// <param name="services">Die Dienstsammlung, die konfiguriert werden soll.</param>
        private void ConfigureServices(IServiceCollection services)
        {
            // Registrierung des DbContext mit einer In-Memory-Datenbank.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDB"));

            // Registrierung der Repositories.
            services.AddSingleton<IAppointmentRepository, AppointmentRepository>();

            // Registrierung der ViewModels.
            services.AddTransient<AppointmentViewModel>();
            services.AddSingleton<WeeklyPlannerViewModel>();

            // Registrierung der Views.
            services.AddTransient<AppointmentModalView>();
            services.AddTransient<WeeklyPlannerView>();

            // Registrierung des Hauptfensters.
            services.AddSingleton<MainWindow>();
        }

        private readonly IServiceProvider serviceProvider;
    }
}
