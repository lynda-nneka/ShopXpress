using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopXpress.Models.Entities;
using ShopXpress.Services.Interfaces;
using System.Xml.Linq;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.DTOs.Requests;
using System.Security.Claims;

namespace ShopXpress.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }


        


        [HttpPost("add-product-to-cart", Name = "add-product-to-cart")]
        public async Task<CartResponse> CreateCart([FromBody] AddToCartRequest request)
        {


            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userIdGuid = Guid.TryParse(userId, out var parsedGuid) ? parsedGuid : Guid.Empty;


            var response = await _cartService.CreateCart(userIdGuid, request.ProductId, request.Quantity);
            return response;
        }


       

        [HttpDelete("delete-product/{productId}", Name = "delete-product-from-cart")]
        public async Task<CartResponse> DeleteProduct(string productId)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userIdGuid = Guid.TryParse(userId, out var parsedGuid) ? parsedGuid : Guid.Empty;

            var response = await _cartService.RemoveCartItemFromCart(userIdGuid, productId);
            return response;
        }

        [HttpGet("view-items-in-cart/{cartId}")]
        public async Task<List<CartItem>> ViewItemsInCart(string cartId)
        {
            var cartItems = await _cartService.ViewItemsInCart(cartId);
            return cartItems;
        }
    }

}

