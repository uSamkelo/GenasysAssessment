using GenasysAssessment.Models;
using GenasysAssessment.Services.Interfaces;

namespace GenasysAssessment.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        public PaymentResult ProcessPayment(int orderId, decimal amount)
        {
            // Simulate payment processing
            if (amount <= 0)
                return new PaymentResult { Success = false, Error = "Invalid amount." };
            // Randomly fail for demonstration
            if (amount > 10000) // Simulate failure for large amounts
                return new PaymentResult { Success = false, Error = "Payment declined." };
            return new PaymentResult { Success = true, TransactionId = Guid.NewGuid().ToString() };
        }
    }
}
