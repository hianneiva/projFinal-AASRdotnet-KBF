using KnowledgeBaseForum.Commons.JWT.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KnowledgeBaseForum.Commons.JWT.Impl
{
    /// <summary>
    /// Implements operations related to the JWT Token generation and use.
    /// </summary>
    /// <typeparam name="T">Class used in token generation.</typeparam>
    public class TokenService : ITokenService
    {
        private const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fffffff zzz";
        private const string JWT_CUSTOM_EXP_NAME = "ExpectedExpiration";

        /// <summary>
        /// Options for JWT operations.
        /// </summary>
        public JwtOptions Options { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options">Options used in JWT operations.</param>
        public TokenService(IOptions<JwtOptions> options)
        {
            Options = options.Value;
        }

        /// <summary>
        /// Generates a JWT Token.
        /// </summary>
        /// <param name="user">The user to whose token will be generated.</param>
        /// <param name="claims">The list of claims from the original token, if applicable.</param>
        /// <returns>The generated JWT Token.</returns>
        public IEnumerable<Claim> ExtractClaimsFromToken(string token)
        {
            TokenValidationParameters validationParams = new()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.Secret)),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false
            };
            JwtSecurityTokenHandler handler = new();
            ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token, validationParams, out SecurityToken validatedToken);

            if (validatedToken is not JwtSecurityToken jwtValidatedToken ||
                !jwtValidatedToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return claimsPrincipal.Claims;
        }

        /// <summary>
        /// Generates a JWT Token.
        /// </summary>
        /// <param name="user">The user to whose token will be generated.</param>
        /// <param name="claims">The list of claims from the original token, if applicable.</param>
        /// <returns>The generated JWT Token.</returns>
        public string GenerateToken(UserContainer? user, IEnumerable<Claim>? claims)
        {
            if (user == null && claims?.Count() == 0)
            {
                throw new ArgumentNullException("Não existem dados de usuário da sessão");
            }

            DateTime expirationDateTime = DateTime.Now.AddMinutes(Options.Expires > 0 ? Options.Expires : 480);
            List<Claim> tokenClaims;
            DateTime? toExpire = claims != null ?
                                 DateTime.ParseExact(claims!.First(c => c.Type.Equals(JWT_CUSTOM_EXP_NAME)).Value, DATETIME_FORMAT, CultureInfo.InvariantCulture) :
                                 null;

            if (toExpire.HasValue)
            {
                if (toExpire.GetValueOrDefault().AddMinutes(10) >= DateTime.Now)
                {
                    return string.Empty;
                }
                else
                {
                    tokenClaims = new(claims!);
                    tokenClaims.RemoveAll(c => c.Type.Equals(JWT_CUSTOM_EXP_NAME));
                    tokenClaims.Add(new Claim(JWT_CUSTOM_EXP_NAME, expirationDateTime.ToString(DATETIME_FORMAT)));
                }
            }
            else
            {
                tokenClaims = new() {
                    new Claim(ClaimTypes.Name, user!.Username),
                    new Claim(ClaimTypes.GivenName, user!.GivenName),
                    new Claim(ClaimTypes.Role, user!.Profile == 1 ? "NORMAL" : "ADMIN"),
                    new Claim(ClaimTypes.Email, user!.Email),
                    new Claim(JWT_CUSTOM_EXP_NAME, expirationDateTime.ToString(DATETIME_FORMAT))
                };
            }

            JwtSecurityTokenHandler handler = new();
            byte[] key = Encoding.UTF8.GetBytes(Options.Secret);
            SecurityTokenDescriptor descriptor = new()
            {
                Expires = expirationDateTime,
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(tokenClaims)
            };
            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
