using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace ShopXpress.Models.Entities
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        
        public string FullName { get; set; } 
    }
}

