using GenasysAssessment.Models;
using GenasysAssessment.Services.Interfaces;

namespace GenasysAssessment.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        public PaymentResult ProcessPayment(int orderId, decimal amount)
        {
            throw new NotImplementedException(); // WIP to be implemented later for processing payments
        }
    }
}
