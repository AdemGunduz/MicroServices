using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Course.Shread.Services
{

    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SharedIdentityService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public string GetUserId
        {
            get
            {
                var user = _contextAccessor.HttpContext?.User;
                if (user == null)
                {
                    throw new InvalidOperationException("HttpContext or User is null.");
                }

         
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    throw new InvalidOperationException("The 'sub' claim is missing.");
                }

                return userIdClaim.Value;
            }
        }
    }
}

