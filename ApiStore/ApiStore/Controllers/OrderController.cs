using ApiStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Microsoft.AspNetCore.Components.Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ApiStoreDbContext _dbContext;

    public OrderController(ApiStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("create")]
    public IActionResult CreateOrder()
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        var cart = _dbContext.Carts.Include(c => c.CartItems)
                                   .ThenInclude(ci => ci.Product)
                                   .FirstOrDefault(c => c.UserId == userId);

        if (cart == null || !cart.CartItems.Any())
            return BadRequest("Кошик порожній.");

        var order = new OrderEntity
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            Status = "Pending",
            OrderItems = cart.CartItems.Select(ci => new OrderItemEntity
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity
            }).ToList()
        };

        _dbContext.Orders.Add(order);
        _dbContext.CartItems.RemoveRange(cart.CartItems);
        _dbContext.SaveChanges();

        return Ok(order);
    }
}
