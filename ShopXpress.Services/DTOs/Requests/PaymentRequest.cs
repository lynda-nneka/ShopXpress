using System;
namespace ShopXpress.Services.DTOs.Requests;

public class PaymentRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Amount { get; set; }
}

