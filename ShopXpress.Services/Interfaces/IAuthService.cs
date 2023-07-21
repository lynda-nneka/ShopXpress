using System;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;

namespace ShopXpress.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<RegisterResponse> CreateAccount(RegisterRequest request);
        Task CreateRole(CreateRoleRequest request);
        
    }
}

