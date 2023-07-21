using System;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using ShopXpress.Models.Entities;
using ShopXpress.Models.Enums;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IMongoCollection<Product> _productService;
        private readonly IMongoCollection<Cart> _cartService;
        private readonly IMongoCollection<Order> _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public OrderService(IMongoDatabase database, IHttpContextAccessor httpContextAccessor)
        {
            _productService = database.GetCollection<Product>("products");
            _cartService = database.GetCollection<Cart>("cart");
            _orderService = database.GetCollection<Order>("order");
            _httpContextAccessor = httpContextAccessor;
           

        }

        public async Task<OrderResponse> PlaceOrder(OrderRequest request, Guid userId)
        {
            var existingCart = _cartService.Find(cart => cart.UserId == userId).FirstOrDefault();


            if (existingCart == null || !existingCart.CartItems.Any())
            {
                throw new InvalidOperationException("cart does not exist or is empty");


            }


            
                var order = new Order()
                {


                    CartId = existingCart.Id,
                    UserId = existingCart.UserId,
                    DeliveryFee = 5000,
                    Total = existingCart.Total,
                    TotalItems = (existingCart.Total + 5000),
                    DeliveryMethod = request.DeliveryMethod,
                    DeliveryAddress = request.DeliveryAddress


                };
                await _orderService.InsertOneAsync(order);

                return new OrderResponse
                {
                    Success = true,
                    Message = "order placed successfully"
                };
            
        }
    }
}
