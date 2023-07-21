using System;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ShopXpress.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<RegisterResponse> CreateAccount(RegisterRequest request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.Email);
                if(userExists != null)
                {
                    return new RegisterResponse
                    {
                        Message = "User already exists",
                        Success = false
                    };
                }

                userExists = new ApplicationUser
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    UserName = request.Username
                };

                var createUserResult = await _userManager.CreateAsync(userExists, request.Password);
                if (!createUserResult.Succeeded)
                    return new RegisterResponse
                    {
                        Message = $"user creation failed{ createUserResult?.Errors?.First()?.Description}",
                        Success = false
                    };

                var addUserToRoleResult = await _userManager.AddToRoleAsync(userExists, "User");
                if (!addUserToRoleResult.Succeeded)
                    return new RegisterResponse
                    {
                        Message = $"user creation succeeded but could not add user to role{createUserResult?.Errors?.First()?.Description}",
                        Success = false
                    };

                return new RegisterResponse
                {
                    Success = true,
                    Message = "user registered successfully"

                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return new LoginResponse
                    {
                        Message = "Invalid email or password",
                        Success = false
                    };
                }

                var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                claims.AddRange(roleClaims);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdf;lkjtebnshsdkds"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(30);

                var token = new JwtSecurityToken(
                    issuer: "http://localhost:24946",
                    audience: "http://localhost:24946",
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                    );

                return new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = "Login Succcessful",
                    Email = user?.Email,
                    Success = true,
                    UserId = user?.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        public async Task CreateRole(CreateRoleRequest request)
        {
            var appRole = new ApplicationRole
            {
                Name = request.Role
            };
            var createdRole = await _roleManager.CreateAsync(appRole);
        }
    }
}

