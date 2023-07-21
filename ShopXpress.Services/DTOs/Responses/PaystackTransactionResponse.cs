using System;
namespace ShopXpress.Services.DTOs.Responses
{
    public class PaystackTransactionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }

    }
}

