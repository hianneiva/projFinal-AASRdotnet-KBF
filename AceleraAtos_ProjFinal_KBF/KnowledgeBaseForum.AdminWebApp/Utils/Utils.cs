using System.Security.Claims;
using static KnowledgeBaseForum.AdminWebApp.Utils.Constants;

namespace KnowledgeBaseForum.AdminWebApp.Utils
{
    /// <summary>
    /// Miscellaneous operations to be used app wide.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Gets the JWT token saved in the cookies.
        /// </summary>
        /// <param name="reqCookies">The request Cookie collection.</param>
        /// <returns>The JWT Token.</returns>
        public static string? GetTokenFromCookies(this IRequestCookieCollection reqCookies) => reqCookies.ContainsKey(JWT_COOKIE_KEY) ?
                                                                                               reqCookies[JWT_COOKIE_KEY] :
                                                                                               null;

        /// <summary>
        /// Gets the username from the currently authenticated user's claims.
        /// </summary>
        /// <param name="claims">User's claims.</param>
        /// <returns>User's username.</returns>
        public static string? GetUserNameFromClaims(this IEnumerable<Claim> claims)
        {
            string username = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name))?.Value ?? string.Empty;
            return username;
        }

        /// <summary>
        /// Gets the given name from the currently authenticated user's claims.
        /// </summary>
        /// <param name="claims">User's claims.</param>
        /// <returns>User's given name.</returns>
        public static string? GetGivenNameFromClaims(this IEnumerable<Claim> claims)
        {
            string givenName = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.GivenName))?.Value ?? string.Empty;
            return givenName;
        }
    }
}
