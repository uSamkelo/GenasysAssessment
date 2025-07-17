using GenasysAssessment.Data;
using GenasysAssessment.Models;
using GenasysAssessment.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace GenasysAssessment.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDataContext _context;
        private readonly IInventoryService _inventoryService;
        private readonly IPaymentService _paymentService;

        public OrdersController(
            ApplicationDataContext context,
            IInventoryService inventoryService,
            IPaymentService paymentService)
        {
            _context = context;
            _inventoryService = inventoryService;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequest request)
        {
            if (request == null || request.ProductId <= 0 || request.Quantity <= 0)
                return BadRequest("Invalid order data.");

            var order = new Order
            {
                CustomerId = 0, // Set a default or get from request if available
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = request.ProductId,
                        Quantity = request.Quantity,
                        UnitPrice = 10 // Assume price=10 for demo
                    }
                },
                TotalAmount = request.Quantity * 10,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // 1. Check inventory
            if (!_inventoryService.CheckAvailability(request.ProductId, request.Quantity))
            {
                order.Status = "Failed: Insufficient inventory";
                return Conflict(new { error = "Insufficient inventory." });
            }

            // 2. Reserve inventory
            if (!_inventoryService.Reserve(request.ProductId, request.Quantity))
            {
                order.Status = "Failed: Inventory reservation error";
                return Conflict(new { error = "Unable to reserve inventory." });
            }

            // 3. Process payment
            var paymentResult = _paymentService.ProcessPayment(0, request.Quantity * 10);
            if (!paymentResult.Success)
            {
                _inventoryService.Release(request.ProductId, request.Quantity);
                order.Status = $"Failed: Payment error - {paymentResult.Error}";
                return StatusCode(402, new { error = paymentResult.Error });
            }

            // 4. Confirm order
            order.Status = "Confirmed";
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> ListOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var items = await _context.Orders
                .Include(o => o.Items)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(items);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            order.Status = request.Status ?? order.Status;
            await _context.SaveChangesAsync();
            return Ok(order);
        }
    }
}
