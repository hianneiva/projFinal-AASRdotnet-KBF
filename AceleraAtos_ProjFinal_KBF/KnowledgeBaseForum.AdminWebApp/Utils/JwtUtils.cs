using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KnowledgeBaseForum.AdminWebApp.Utils
{
    /// <summary>
    /// Class that implements JWT related perationso.
    /// </summary>
    public static class JwtUtils
    {
        public static IEnumerable<Claim> DecodeJwt(string key, string jwtToken, string issuer)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParams = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            handler.ValidateToken(jwtToken, validationParams, out SecurityToken validatedToken);
            JwtSecurityToken? decodedToken = validatedToken as JwtSecurityToken;

            if (decodedToken == null)
            {
                throw new ArgumentNullException("Falha na validação");
            }

            return decodedToken.Claims;
        }
    }
}
