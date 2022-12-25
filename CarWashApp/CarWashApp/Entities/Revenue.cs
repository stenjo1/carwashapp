namespace CarWashApp.Entities
{
    public class Revenue
    {
        public int CarWashId { get; set; }
        public double CurrentValue { get; set; } = 0d;
        public double DailyIncome { get; set; } = 0d;
        public double WeeklyIncome { get; set; } = 0d;
        public double MonthlyIncome { get; set; } = 0d;
        public CarWash CarWash { get; set; }
    }
}
