namespace CarWashApp.DTOs
{
    public class AppointmentFilter
    {
        public int CarWashId { get; set; }
        public bool IsFinished { get; set; }
        public uint Rating { get; set; } = 0u;
        public string? ServiceType { get; set; }
        public string? Status { get; set; }
        public string? OrderingField { get; set; }
        public bool Ascending { get; set; }
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;
    }
}
