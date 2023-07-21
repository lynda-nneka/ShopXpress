using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ShopXpress.Models.Entities
{
    [CollectionName("roles")]
    public class ApplicationRole : MongoIdentityRole<Guid>
    {
        public ApplicationRole()
        {
        }
    }
}

