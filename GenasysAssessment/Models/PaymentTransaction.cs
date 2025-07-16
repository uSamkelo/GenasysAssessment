namespace GenasysAssessment.Models
{
    public class PaymentTransaction
    {
        public string TransactionId { get; set; } 
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
