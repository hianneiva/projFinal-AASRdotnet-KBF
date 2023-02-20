using KnowledgeBaseForum.Commons.JWT.Model;
using System.Security.Claims;

namespace KnowledgeBaseForum.Commons.JWT
{
    /// <summary>
    /// Token Service Interface for dependency Injection.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Extract claims from token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>The found claims.</returns>
        public IEnumerable<Claim> ExtractClaimsFromToken(string token);

        /// <summary>
        /// Generates a JWT Token.
        /// </summary>
        /// <param name="user">The user to whose token will be generated.</param>
        /// <param name="claims">The list of claims from the original token, if applicable.</param>
        /// <returns>The generated JWT Token.</returns>
        public string GenerateToken(UserContainer? user, IEnumerable<Claim>? claims);
    }
}
