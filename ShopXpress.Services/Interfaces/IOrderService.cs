using System;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;

namespace ShopXpress.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse> PlaceOrder(OrderRequest request, Guid userId);
    }
}

