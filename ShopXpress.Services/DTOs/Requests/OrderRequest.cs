using System;
using ShopXpress.Models.Enums;

namespace ShopXpress.Services.DTOs.Requests
{
    public class OrderRequest
    {
        public DeliveryMethod DeliveryMethod { get; set; }
        public string DeliveryAddress { get; set; }
    }
}

