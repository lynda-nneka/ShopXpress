using System;
using MongoDB.Driver;
using ShopXpress.Models.Entities;
using ShopXpress.Services.Interfaces;
//using ShopXpress.Data.Implementations;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System.Xml;
using ShopXpress.Data.Implementations;

namespace ShopXpress.Services.Implementations
{
    public class ProductService : IProductService
    {
      
        private readonly IMongoCollection<Product>_productService;

        public ProductService(IMongoDatabase database)
        {
            _productService = database.GetCollection<Product>("products");

            var indexKeysDefinition = Builders<Product>.IndexKeys.Text(x => x.Name).Text(x => x.Description);
            var options = new CreateIndexOptions { Name = "TextIndex" };
            var model = new CreateIndexModel<Product>(indexKeysDefinition, options);
            _productService.Indexes.CreateOne(model);

        }

 
        public async Task<Product> CreateProduct(Product product)
        {
            await _productService.InsertOneAsync(product);
            return new Product
            {
                Id = product.Id,
                UserId = product.UserId,
                Name = product.Name,
                Quantity = product.Quantity,
                Price = product.Price,
                Description = product.Description,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task DeleteProduct(string Id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("Id", Id);
            await _productService.DeleteOneAsync(filter);
                return;
        }

        public Task<Product> GetProduct(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _productService.Find(new BsonDocument()).ToListAsync();
            return products;
        }

        public async Task<Product> UpdateProduct(string Id, Product product)
        {
            var existingProduct = _productService.Find(product => product.Id == Id).FirstOrDefault();

            if (existingProduct == null)
            {
                return null;
            }

            product.Id = Id;

            _productService.ReplaceOne(product => product.Id == Id, product);

            return product;
        }


        public async Task<IEnumerable<Product>> Search(string query)
        {
            var filter = Builders<Product>.Filter.Text(query);
            var entity = await _productService.Find(filter).ToListAsync();

            return entity;
        }
    }
}

