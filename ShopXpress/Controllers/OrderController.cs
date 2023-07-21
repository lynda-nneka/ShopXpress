using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Implementations;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(IOrderService orderService, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;

        }


        [HttpPost("place-order", Name = "place-order")]
        public async Task<OrderResponse> PlaceOrder([FromBody] OrderRequest request)
        {


            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userIdGuid = Guid.TryParse(userId, out var parsedGuid) ? parsedGuid : Guid.Empty;


            var response = await _orderService.PlaceOrder(request, userIdGuid);
            return response;
        }

    }
}
