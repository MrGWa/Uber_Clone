namespace UberClone.Core.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Type { get; set; } // e.g., "Credit Card", "PayPal"
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
    }
}
