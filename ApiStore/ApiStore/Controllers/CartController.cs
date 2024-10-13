using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiStore.Data;
using System.Collections.Generic;
using System;

namespace ApiStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ApiStoreDbContext _dbContext;

        public CartController(ApiStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        [HttpPost("add-to-cart")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var cart = _dbContext.Carts.Include(c => c.CartItems)
                                       .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new CartEntity { UserId = userId, CartItems = new List<CartItemEntity>() };
                _dbContext.Carts.Add(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                cartItem = new CartItemEntity { ProductId = productId, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            _dbContext.SaveChanges();
            return Ok();
        }


        [HttpGet("cart-items")]
        public IActionResult GetCartItems()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var cartItems = _dbContext.Carts
                                      .Where(c => c.UserId == userId)
                                      .SelectMany(c => c.CartItems)
                                      .Include(ci => ci.Product)
                                      .ToList();

            return Ok(cartItems);
        }



        [HttpPost("create-order")]
        public IActionResult CreateOrder()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var cart = _dbContext.Carts.Include(c => c.CartItems)
                                       .ThenInclude(ci => ci.Product)
                                       .FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("Кошик порожній.");
            }

            var order = new OrderEntity
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderItems = cart.CartItems.Select(ci => new OrderItemEntity
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList(),
                TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity)
            };

            _dbContext.Orders.Add(order);
            _dbContext.CartItems.RemoveRange(cart.CartItems);
            _dbContext.SaveChanges();

            return Ok(order);
        }
    }
}
