using KnowledgeBaseForum.DataAccessLayer.Model;

namespace KnowledgeBaseForum.API.Model
{
    /// <summary>
    /// Payload for user update request.
    /// </summary>
    public class UpdateUser
    {
        /// <summary>
        /// User entry.
        /// </summary>
        public Usuario? Entry { get; set; }

        /// <summary>
        /// Encoded password.
        /// </summary>
        public string? Password { get; set; }
    }
}
