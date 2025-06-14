// BasicAuthAttribute.cs
using Microsoft.AspNetCore.Authorization;

namespace tns.Server.src.Shared.Authentication
{
    public class BasicAuthAttribute : AuthorizeAttribute
    {
        public BasicAuthAttribute()
        {
            AuthenticationSchemes = "Basic";
        }
    }
}