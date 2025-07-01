namespace UberClone.Core.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int RideId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
