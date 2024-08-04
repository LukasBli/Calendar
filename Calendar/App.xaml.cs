using Calendar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                options.UseSqlServer("Server=DESKTOP-G41ADTG;Database=Calendar;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"));

            // Registering Repositories
            services.AddSingleton<IAppointmentRepository, AppointmentRepository>();

            // Registering ViewModels
            services.AddTransient<AppointmentViewModel>();
            services.AddSingleton<WeeklyPlannerViewModel>();

            // Registering Views
            services.AddTransient<AppointmentModalView>();
            services.AddTransient<AppointmentEntryView>();
            services.AddTransient<WeeklyPlannerView>();

            // Registering MainWindow
            services.AddSingleton<MainWindow>();
        }
    }
}
