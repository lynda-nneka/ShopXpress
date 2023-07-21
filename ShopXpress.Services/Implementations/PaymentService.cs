using System;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PayStack.Net;
using ShopXpress.Data.Implementations;
//using ShopXpress.Data.Implementations;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IMongoCollection<Transaction> _transaction;
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<ApplicationUser> _user;

        private PayStackApi PayStackApi { get; set; }
        private readonly string token;

        public PaymentService(IOptions<DatabaseSettings> mongoDBSettings, IMongoDatabase database)
        {
            _transaction = database.GetCollection<Transaction>("transactions");
            _user = database.GetCollection<ApplicationUser>("Users");
            _order = database.GetCollection<Order>("order");
            token = mongoDBSettings.Value.PayStackSK;
            PayStackApi = new PayStackApi(token);

        }


        public async Task<string> MakePayment(Guid userIdGuid)
        {

            var oneUser = _user.Find(user => user.Id == userIdGuid).FirstOrDefault();



            // Check if the user exists
            if (oneUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var oneOrder = _order.Find(order => order.UserId == userIdGuid).FirstOrDefault();

           

            TransactionInitializeRequest request = new()
            {
                AmountInKobo = (int)oneOrder.Total * 100,
                Email = oneUser.Email,
                Reference = GenerateReference().ToString(),
                Currency = "NGN",
                CallbackUrl = "http://localhost:24946"
            };

            TransactionInitializeResponse response = PayStackApi.Transactions.Initialize(request);
            if (response.Status)
            {
                var transaction = new Transaction
                {
                    Amount = oneOrder.Total,
                    Email = oneUser.Email,
                    Ref = request.Reference,
                    Name = oneUser.FullName

                };
                await _transaction.InsertOneAsync(transaction);

                return response.Data.AuthorizationUrl;
            }
            return "something went wrong";
        }

        public async Task<PaystackTransactionResponse> VerifyPayment(string reference)
        {
            TransactionVerifyResponse response = PayStackApi.Transactions.Verify(reference);
            if(response.Data.Status == "success")
            {
                var transaction = await _transaction.Find(t => t.Ref == reference).FirstOrDefaultAsync();
                if(transaction != null)
                {
                    transaction.Status = true;
                   await _transaction.UpdateOneAsync(Builders<Transaction>.Filter.Eq(t => t.Name, transaction.Name),
                  Builders<Transaction>.Update.Set(t => t.Status, true));
                    
                }
                var paystackResponse = new PaystackTransactionResponse
                {
                    Success = true,
                    Message = "Transaction verified successfully",
                    
                };

                return paystackResponse;
            }

            return new PaystackTransactionResponse
            {
                Success = false,
                Message = "Transaction verification failed",
            };
        }

        public int GenerateReference()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000000, 9999999);
            return randomNumber;
        }
    }
}

