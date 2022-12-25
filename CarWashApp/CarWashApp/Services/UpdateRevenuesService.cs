using Microsoft.EntityFrameworkCore;

namespace CarWashApp.Services
{
    public class UpdateRevenuesService : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private Timer currentTimer;
        private Timer dailyTimer;
        private Timer weeklyTimer;
        private Timer monthlyTimer;

        public UpdateRevenuesService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            currentTimer?.Dispose();
            dailyTimer?.Dispose();
            weeklyTimer?.Dispose();
            monthlyTimer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var currTime = DateTime.Now;
            var timeSpan = DateTime.Today.AddDays(1).AddMilliseconds(-1) - currTime;

            currentTimer = new Timer(UpdateCurrentRevenues, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            dailyTimer = new Timer(UpdateDailyRevenues, null, timeSpan, TimeSpan.FromDays(1));
            weeklyTimer = new Timer(UpdateWeeklyRevenues, null, timeSpan, TimeSpan.FromDays(7));
            monthlyTimer = new Timer(UpdateMonthlyRevenues, null, timeSpan, TimeSpan.FromDays(30));
            return Task.CompletedTask;

            //test
            //var currTime = DateTime.Now;
            //var timeSpan = DateTime.Today.AddHours(12).AddMinutes(24) - currTime;

            //currentTimer = new Timer(UpdateCurrentRevenues, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            //dailyTimer = new Timer(UpdateDailyRevenues, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            //weeklyTimer = new Timer(UpdateWeeklyRevenues, null, TimeSpan.Zero, TimeSpan.FromSeconds(45));
            //monthlyTimer = new Timer(UpdateMonthlyRevenues, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            //return Task.CompletedTask;
        }

        private async void UpdateCurrentRevenues(object state)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var currDateTime = DateTime.Now;
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var appointments = await context.Appointments.Where(x => x.Status == Entities.Status.Approved && (x.Date.AddHours(x.EndHour)) <= currDateTime && !x.IsFinished)
                                                             .Include(x => x.CarWash)
                                                             .ThenInclude(x => x.Services)
                                                             .Include(x => x.CarWash)
                                                             .ThenInclude(x => x.Revenue)
                                                             .Include(x => x.Customer)
                                                             .ToListAsync();
                foreach (var app in appointments)
                {
                    app.IsFinished = true;
                    var servicePrice = app.CarWash.Services.Where(x => x.ServiceType == app.ServiceType).First().Price;
                    app.CarWash.Revenue.CurrentValue += servicePrice;
                    app.Customer.Wallet -= servicePrice;
                }

                await context.SaveChangesAsync();
            }
        }

        private async void UpdateDailyRevenues(object state)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var revenues = await context.Revenues.ToListAsync();
                foreach(var revenue in revenues)
                {
                    revenue.DailyIncome += revenue.CurrentValue;
                    revenue.CurrentValue = 0d;
                }

                await context.SaveChangesAsync();
            }
        }

        private async void UpdateWeeklyRevenues(object state)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var revenues = await context.Revenues.ToListAsync();
                foreach (var revenue in revenues)
                {
                    revenue.WeeklyIncome += revenue.DailyIncome;
                    revenue.DailyIncome = 0d;
                }

                await context.SaveChangesAsync();
            }
        }

        private async void UpdateMonthlyRevenues(object state)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var revenues = await context.Revenues.ToListAsync();
                foreach (var revenue in revenues)
                {
                    revenue.MonthlyIncome += revenue.WeeklyIncome;
                    revenue.WeeklyIncome = 0d;
                }

                await context.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            currentTimer?.Change(Timeout.Infinite, 0);
            dailyTimer?.Change(Timeout.Infinite, 0);
            weeklyTimer?.Change(Timeout.Infinite, 0);
            monthlyTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
