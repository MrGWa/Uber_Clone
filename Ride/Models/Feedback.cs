namespace RideApp.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int RideId { get; set; }
        public int GiverId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // 1 to 5
    }
}
