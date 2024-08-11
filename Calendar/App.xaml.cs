using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WeeklyPlanner.Data.Data;
using WeeklyPlanner.Data.Repositories;
using WeeklyPlanner.UI.ViewModels;
using WeeklyPlanner.UI.Views;

namespace Calendar
{
    public partial class App : Application
    {
        private readonly IServiceProvider mServiceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            mServiceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = mServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Registering DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("CONNECTIONSTRING")); // Alle stellen mit CONNECTIONSTRING müssen den Connectionstring bekommen damit
                                                           // die DB klappt und es muss ein update ausgeführt werden damit die Migrationen ausgeführt werden.

            // Registering Repositories
            services.AddSingleton<IAppointmentRepository, AppointmentRepository>(); 

            // Registering ViewModels
            services.AddTransient<AppointmentViewModel>();
            services.AddSingleton<WeeklyPlannerViewModel>();

            // Registering Views
            services.AddTransient<AppointmentModalView>();
            services.AddTransient<WeeklyPlannerView>();

            // Registering MainWindow
            services.AddSingleton<MainWindow>();
        }
    }
}
