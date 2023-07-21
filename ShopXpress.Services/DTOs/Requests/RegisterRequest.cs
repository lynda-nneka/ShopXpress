using System;
using System.ComponentModel.DataAnnotations;

namespace ShopXpress.Services.DTOs.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string FullName { get; set; }
        public string Username { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}

