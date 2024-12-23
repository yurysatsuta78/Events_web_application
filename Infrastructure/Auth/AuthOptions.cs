using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "WebApi";
        public const string AUDIENCE = "Client";
        const string KEY = "AUDI100AUDI200AUDIA6AUDI80AUDIV8";
        public const int ACCESS_LIFETIME = 15;
        public const int REFRESH_LIFETIME = 7;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() 
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
