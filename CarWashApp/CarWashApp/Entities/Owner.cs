using System.ComponentModel.DataAnnotations;

namespace CarWashApp.Entities
{
    public class Owner : ApplicationUser
    {
        public double TotalIncome { get; set; } = 0d;
        public double TotalLoss { get; set; } = 0d;
        public List<CarWash> CarWashes { get; set; }
    }
}
