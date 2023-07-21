using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ShopXpress.Models.Entities
{
    [CollectionName("transactions")]
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("ref")]
        public string Ref { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [BsonElement("status")]
        public bool Status { get; set; }

    }
}

