using System;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;

namespace ShopXpress.Services.Interfaces
{
    public interface IPaymentService
    {
        
        Task<string> MakePayment(Guid userIdGuid);
        Task<PaystackTransactionResponse> VerifyPayment(string reference);
    }
}

