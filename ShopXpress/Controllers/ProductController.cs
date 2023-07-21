using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopXpress.Models.Entities;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("get-products")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _productService.GetProducts();
            return products;
        }


        [HttpPost("create-new-product", Name = "Create-New-Product")]
        public async Task<Product> CreateProduct([FromBody] Product product)
        {
            var response = await _productService.CreateProduct(product);
            return response;
        }


        [HttpPut("update-product/{Id}")]
        public async Task<Product> UpdateProduct(string Id, [FromBody] Product product)
        {
            var updatedProduct = await _productService.UpdateProduct(Id, product);
            return updatedProduct;
        }

        [HttpDelete("delete-product/{Id}")]
        public async Task DeleteProduct(string Id)
        {
            await _productService.DeleteProduct(Id);
            return;
        }

        [HttpGet("search/{query}")]
        public async Task<IEnumerable<Product>> Search(string query)
        {
            var searchProduct = await _productService.Search(query);
            return searchProduct;
        }
    }
}

