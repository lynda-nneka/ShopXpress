using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ShopXpress.Models.Enums;

namespace ShopXpress.Models.Entities
{
    public class Order
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public Guid UserId { get; set; }
        public string CartId { get; set; }
        public decimal TotalItems { get; set; }
        public Decimal DeliveryFee { get; set; }
        public decimal Total { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public string DeliveryAddress { get; set; }
    }
}

