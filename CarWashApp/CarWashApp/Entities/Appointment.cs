using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWashApp.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int CarWashId { get; set; }
        public string? CustomerId { get; set; }
        [Range(0,24)]
        public int StartHour { get; set; }
        [Range(0, 24)]
        public int EndHour { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsFinished { get; set; } = false;
        [Range(0, 5)]
        public uint Rating { get; set; } = 0u;
        public string ServiceType { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public CarWash? CarWash { get; set; }
        public Customer? Customer { get; set; }
    }

    public enum Status
    {
        Pending,
        Approved,
        Declined
    }
}
