namespace UberClone.Core.Entities
{
    public class Ride
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int DriverId { get; set; }
        public decimal Distance { get; set; }
        public decimal Fare { get; set; }
        public string Status { get; set; } // e.g., "Completed", "In Progress"
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
