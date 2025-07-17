using GenasysAssessment.Models;
using GenasysAssessment.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GenasysAssessment.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDataContext _context;
        public InventoryController(ApplicationDataContext context)
        {
            _context = context;
        }

        [HttpGet("{productId}")]
        public IActionResult CheckAvailability(int productId)
        {
            var item = _context.InventoryItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return NotFound();
            return Ok(new { item.ProductId, item.AvailableQuantity, item.ReservedQuantity });
        }

        [HttpPost("{productId}/reserve")]
        public IActionResult ReserveItem(int productId, [FromBody] int quantity)
        {
            var item = _context.InventoryItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return NotFound();
            if (item.AvailableQuantity < quantity)
                return BadRequest("Insufficient stock available.");
            item.AvailableQuantity -= quantity;
            item.ReservedQuantity += quantity;
            _context.SaveChanges();
            return Ok(new { item.ProductId, item.AvailableQuantity, item.ReservedQuantity });
        }

        [HttpPost("{productId}/release")]
        public IActionResult ReleaseItem(int productId, [FromBody] ReleaseRequest request)
        {
            var item = _context.InventoryItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null || item.ReservedQuantity < request.Quantity)
                return BadRequest("Not enough reserved items.");
            item.AvailableQuantity += request.Quantity;
            item.ReservedQuantity -= request.Quantity;
            _context.SaveChanges();
            return Ok(item);
        }
    }
}
