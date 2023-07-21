using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ShopXpress.Models.Entities
{
    [CollectionName("products")]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("merchantsId")]
        public Guid UserId { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("price")]
        public decimal Price { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}

