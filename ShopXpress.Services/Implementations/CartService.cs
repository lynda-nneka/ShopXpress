using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using PayStack.Net;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IMongoCollection<Product> _productService;
        private readonly IMongoCollection<Cart> _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IMongoDatabase database, IHttpContextAccessor httpContextAccessor)
        {
            _productService = database.GetCollection<Product>("products");
            _cartService = database.GetCollection<Cart>("cart");
            _httpContextAccessor = httpContextAccessor;

        }


        public async Task<CartResponse> CreateCart(Guid userIdGuid, string productId, int quantity)
        {
            var sessionID = _httpContextAccessor.HttpContext.Session.Id;

            //check and verify product status
            var existingProduct = _productService.Find(product => product.Id == productId).FirstOrDefault();
            if (existingProduct is null)
                return new CartResponse
                {
                    Success = false,
                    Message = "Product either does not exist or has been deleted"
                };

            if (existingProduct.UserId == userIdGuid)
                return new CartResponse
                {
                    Message = "You cannot buy your own products",
                    Success = false
                };

            if (existingProduct.Quantity < quantity)
                return new CartResponse
                {
                    Message = $"OOPS! Insufficient Products We currently have {existingProduct.Quantity} of {existingProduct.Name} in our store, your order exceeds that! Reduce your order and try again or check again in a few days after restock",
                    Success = false
                };

            decimal total = 0;
            //check cart status
            var existingCart = _cartService.Find(cart => cart.UserId == userIdGuid).FirstOrDefault();


            if (existingCart == null)
            {

                existingCart = new Cart()
                {
                    UserId = userIdGuid,
                    SessionId = sessionID,
                    Total = existingProduct.Price * quantity,
                    CartItems = new List<CartItem>()

                };



                var cartItem = new CartItem
                {
                    UserId = userIdGuid,

                    Quantity = quantity,
                    Price = existingProduct.Price,
                    ProductId = productId,
                    SubTotal = existingProduct.Price * quantity
                };


                existingCart.CartItems.Add(cartItem);
                
                await _cartService.InsertOneAsync(existingCart);


                return new CartResponse
                {
                    Message = $"cart has been created with {quantity} quantity of {existingProduct.Name}",
                    Success = true
                };
            }


            else  
            {
                //get index of cart item in cart collection
                int indx = 0;
                CartItem cartItem = new();
                
                for (var i = 0; i < existingCart.CartItems.Count; i++)
                {
                   
                    if (existingCart.CartItems[i].ProductId == productId)
                    {
                        

                        cartItem = existingCart.CartItems[i];
                        indx = i;

                        cartItem.Quantity += quantity;
                        cartItem.SubTotal += (cartItem.Price * quantity);
                        existingCart.Total = existingCart.CartItems.Sum(item => item.SubTotal);
                        

                        var cartItemFilter = Builders<Cart>.Filter.Eq("userId", existingCart.UserId) &
                        Builders<Cart>.Filter.ElemMatch(x => x.CartItems, elem => elem.ProductId == productId);

                        var updateList = Builders<Cart>.Update
                                         .Set(x => x.CartItems[indx].Quantity, cartItem.Quantity)
                                         .Set(x => x.CartItems[indx].SubTotal, cartItem.SubTotal)
                                         .Set(x => x.Total, existingCart.Total);
                        await _cartService.UpdateOneAsync(cartItemFilter, updateList);

                        return new CartResponse
                        {
                            Success = true,
                            Message = $"Cart item: Quantity increased"

                        };

                    }

                    else if(existingCart.CartItems[i].ProductId != productId)
                    {

                        continue;


                    }


                    

                   
                }

                var newcartItem = new CartItem
                {
                    UserId = userIdGuid,
                    Quantity = quantity,
                    Price = existingProduct.Price,
                    ProductId = productId,
                    SubTotal = existingProduct.Price * quantity
                };
                existingCart.CartItems.Add(newcartItem);

                existingCart.Total = existingCart.CartItems.Sum(item => item.SubTotal);
                var newCartItemFilter = Builders<Cart>.Filter.Eq("userId", existingCart.UserId);

                var newUpdateList = Builders<Cart>.Update
                    .Set(x => x.CartItems, existingCart.CartItems)
                    .Set(x => x.Total, existingCart.Total);

                await _cartService.UpdateOneAsync(newCartItemFilter, newUpdateList);



                return new CartResponse
                {
                    Success = true,
                    Message = $"{quantity} unit(s) of item Added"
                };


            }
         }



        public async Task<List<CartItem>> ViewItemsInCart(string cartId)
        {
            var existingCart = _cartService.Find(cart => cart.Id == cartId).FirstOrDefault();
            if (existingCart is null)
            {
                throw new InvalidOperationException("cart does not exist");
            }

            bool hasItemsLessThanOne = existingCart.CartItems.Any(item => item.Quantity < 1);

            if (hasItemsLessThanOne)
            {
                throw new InvalidOperationException("cart is empty");
            }

            decimal totalCartPrice = 0;
            foreach (var cartItem in existingCart.CartItems)
            {
                var productFilter = Builders<Product>.Filter.Eq(p => p.Id, cartItem.ProductId);
                var product = await _productService.Find(productFilter).FirstOrDefaultAsync();

                if (product != null)
                {
                    totalCartPrice += cartItem.Price * cartItem.Quantity;
                }


                throw new InvalidOperationException("cart is empty");

            }
            List<CartItem> cartItems = existingCart.CartItems;

            return new List<CartItem>();
            

            
        }


        public async Task<CartResponse> RemoveCartItemFromCart(Guid userId, string productId)
        {
            var existingCart = _cartService.Find(cart => cart.UserId == userId).FirstOrDefault();
            if (existingCart is null)
            {
                return new CartResponse
                {
                    Success = false,
                    Message = "Cart does not exist"

                };
            }
            int index = 0;
            CartItem cartItem = new();
            for (int i = 0; i < existingCart.CartItems.Count; i++)
            {
                if (existingCart.CartItems[i].ProductId == productId)
                {
                    cartItem = existingCart.CartItems[i];
                    index = i;
                    break;
                }

            }

            if(cartItem is null)
            {
                return new CartResponse
                {
                    Success = false,
                    Message = "Product does not exist"
                };
            }

            if (cartItem.Quantity > 1)
            {
                var ItemFilter = Builders<Cart>.Filter.Eq("userId", existingCart.UserId) &
               Builders<Cart>.Filter.ElemMatch(x => x.CartItems, elem => elem.ProductId == productId);
               

                var update = Builders<Cart>.Update.Set(x => x.CartItems[index].Quantity, cartItem.Quantity - 1);

                await _cartService.UpdateOneAsync(ItemFilter, update);
            }

            var cartItemFilter = Builders<Cart>.Filter.Eq("userId", existingCart.UserId);
              
            var updateList = Builders<Cart>.Update.Pull(x => x.CartItems, cartItem);

            await _cartService.UpdateOneAsync(cartItemFilter, updateList); 

      

            return new CartResponse
            {
                Success = true,
                Message = $"Cart item: item deleted"

            };

            
        }
    }
}