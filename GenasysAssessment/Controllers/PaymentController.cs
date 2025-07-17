using GenasysAssessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace GenasysAssessment.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private static readonly Dictionary<string, PaymentTransaction> Transactions = new();

        [HttpPost("process")]
        public IActionResult ProcessPayment([FromBody] PaymentRequest request)
        {
            var transaction = new PaymentTransaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                OrderId = request.OrderId,
                Amount = request.Amount,
                Status = "Processed"
            };
            Transactions[transaction.TransactionId] = transaction;
            return Ok(transaction);
        }

        [HttpGet("{transactionId}")]
        public IActionResult GetPaymentStatus(string transactionId)
        {
            if (!Transactions.TryGetValue(transactionId, out var transaction))
                return NotFound();
            return Ok(transaction);
        }
    }
}
