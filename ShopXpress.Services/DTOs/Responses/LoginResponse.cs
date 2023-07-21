using System;
namespace ShopXpress.Services.DTOs.Responses
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

