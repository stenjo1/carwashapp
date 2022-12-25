using Microsoft.EntityFrameworkCore;

namespace CarWashApp.Services
{
    public class UpdateAppointmentsService : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private Timer updateTimer;
        private Timer deleteDeclinedTimer;
        private Timer deleteTimer;

        public UpdateAppointmentsService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            updateTimer?.Dispose();
            deleteDeclinedTimer?.Dispose();
            deleteTimer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            updateTimer = new Timer(UpdateStatus, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            deleteDeclinedTimer = new Timer(DeleteDeclinedAppointments, null, TimeSpan.Zero, TimeSpan.FromDays(7));
            deleteTimer = new Timer(DeleteAppointments, null, TimeSpan.Zero, TimeSpan.FromDays(30));
            return Task.CompletedTask;

            //test
            //timer = new Timer(UpdateStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            //deleteDeclinedTimer = new Timer(DeleteDeclinedAppointments, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            //deleteTimer = new Timer(DeleteAppointments, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            //return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            updateTimer?.Change(Timeout.Infinite, 0);
            deleteDeclinedTimer?.Change(Timeout.Infinite, 0);
            deleteTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void UpdateStatus(object state)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var appointments = await context.Appointments.Where(x => x.Status == Entities.Status.Pending)
                                                             .ToListAsync();
                foreach (var app in appointments)
                {
                    app.Status = Entities.Status.Approved;
                }

                await context.SaveChangesAsync();
            }
        }

        private async void DeleteDeclinedAppointments(object state)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var appointments = await context.Appointments.Where(x => (x.DateCreated.AddDays(7) <= DateTime.Now) && x.Status == Entities.Status.Declined)
                                                             .ToListAsync();
                foreach (var app in appointments)
                {
                    context.Remove(app);
                }

                await context.SaveChangesAsync();
            }
        }

        private async void DeleteAppointments(object state)
        {
            using(var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var appointments = await context.Appointments.Where(x => x.DateCreated.AddYears(2) <= DateTime.Now)
                                                             .ToListAsync();
                foreach (var app in appointments)
                {
                    context.Remove(app);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
