using System;
using System.ComponentModel.DataAnnotations;

namespace ShopXpress.Services.DTOs.Requests
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

