namespace GenasysAssessment.Models
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public string? Error { get; set; }
    }
}
