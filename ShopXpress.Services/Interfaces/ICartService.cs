using System;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Responses;

namespace ShopXpress.Services.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItem>> ViewItemsInCart(string cartId);
        Task<CartResponse> CreateCart(Guid userIdGuid, string productId, int quantity);
        Task<CartResponse> RemoveCartItemFromCart(Guid userIdGuid, string cartItemId);
        
        
    }
}

