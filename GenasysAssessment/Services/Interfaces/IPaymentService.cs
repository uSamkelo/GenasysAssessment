using GenasysAssessment.Models;

namespace GenasysAssessment.Services.Interfaces
{
    public interface IPaymentService
    {
        PaymentResult ProcessPayment(int orderId, decimal amount);
    }
}
