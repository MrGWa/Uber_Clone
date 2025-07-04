namespace UberClone.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public Guid RideId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = default!;
        public bool IsSuccessful { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
