namespace CarWashApp.DTOs
{
    public class CarWashFilterDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;
        public int Rating { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public bool IsOpen { get; set; }
        public string? OrderingField { get; set; }
        public bool Ascending { get; set; }
        private int distanceInKms = 10;
        private int maxDistanceInKms = 50;

        public int DistanceInKms
        {
            get { return distanceInKms; }
            set { distanceInKms = value > maxDistanceInKms ? maxDistanceInKms : value; }
        }
    }
}
