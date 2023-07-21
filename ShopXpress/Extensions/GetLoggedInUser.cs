using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ShopXpress.Extensions
{
    public class LoggedInUser : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedInUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLoggedInUser()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
        }
    }
}

