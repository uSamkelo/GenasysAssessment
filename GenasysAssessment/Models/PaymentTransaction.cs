using System.ComponentModel.DataAnnotations;

namespace GenasysAssessment.Models
{
    public class PaymentTransaction
    {
        [Key]
        //[MaxLength(36)] removed for SQLite compatibility
        public string TransactionId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime ProcessedAt { get; set; }
    }
}
