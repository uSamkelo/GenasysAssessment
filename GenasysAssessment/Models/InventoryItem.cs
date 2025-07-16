namespace GenasysAssessment.Models
{
    public class InventoryItem
    {
        public string? ProductId { get; set; }
        public int AvailableQuantity { get; set; }
        public int ReservedQuantity { get; set; }
    }
}
