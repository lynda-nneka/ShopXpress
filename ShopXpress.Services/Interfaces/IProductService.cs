using System;
using System.Transactions;
using ShopXpress.Models.Entities;

namespace ShopXpress.Services.Interfaces
{
    public interface IProductService
    {

        Task<Product> CreateProduct(Product product);
        Task<Product> GetProduct(string Id);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> UpdateProduct(string Id, Product product);
        Task DeleteProduct(string Id);
        Task<IEnumerable<Product>> Search(string query);
    }
}

