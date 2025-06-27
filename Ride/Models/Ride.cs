namespace RideApp.Models
{
    public class Ride
    {
        public int RideId { get; set; }
        public int PassengerId { get; set; }
        public int? DriverId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public string Status { get; set; } // Requested, Accepted, InProgress, Completed, Cancelled
        public double? Fare { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}