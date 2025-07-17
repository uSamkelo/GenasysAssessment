using System.ComponentModel.DataAnnotations;

namespace GenasysAssessment.Models
{
    public class OrderItem
    {
        [Key]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
