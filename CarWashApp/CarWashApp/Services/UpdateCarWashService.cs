using Microsoft.EntityFrameworkCore;

namespace CarWashApp.Services
{
    public class UpdateCarWashService : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private Timer timer;

        public UpdateCarWashService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(UpdateRating, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void UpdateRating(object state)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var carWashes = await context.CarWashes.Include(x => x.Appointments).ToListAsync();

                foreach (var carWash in carWashes)
                {
                    foreach (var app in carWash.Appointments)
                    {
                        if (app.Rating != 0)
                        {
                            carWash.Rating += (int)app.Rating;
                            carWash.Votes++;
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
