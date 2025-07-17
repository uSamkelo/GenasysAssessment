namespace GenasysAssessment.Services.Interfaces
{
    public interface IInventoryService
    {
        bool CheckAvailability(int productId, int quantity);
        bool Reserve(int productId, int quantity);
        void Release(int productId, int quantity);
    }
}
