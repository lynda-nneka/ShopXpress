using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Implementations;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class PaymentController : Controller
   {
       private readonly IPaymentService _paymentService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentController(IPaymentService paymentService, IHttpContextAccessor httpContextAccessor)
       {
          _paymentService = paymentService;
            _httpContextAccessor = httpContextAccessor;
        }

       [HttpPost("make-payment", Name = "make-payment")]
       public async Task<string> MakePayment()
       {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userIdGuid = Guid.TryParse(userId, out var parsedGuid) ? parsedGuid : Guid.Empty;

            var response = await _paymentService.MakePayment(userIdGuid);
            return response;


        }

       [HttpGet("payment-verification", Name = "payment-verification")]
       public async Task<PaystackTransactionResponse> VerifyPayment(string reference)
       {
          var response = await _paymentService.VerifyPayment(reference);
          return response;
       }
    }
}

