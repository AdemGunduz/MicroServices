using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Shread.Services
{
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }
}
