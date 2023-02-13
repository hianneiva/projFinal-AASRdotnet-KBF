namespace KnowledgeBaseForum.API.JWT
{
    public class ClaimRoleNames
    {
        public static readonly IDictionary<int, string> USER_ROLE_NAMES = new Dictionary<int, string>()
        {
            { 1, "NORMAL" },
            { 2, "ADMIN" }
        };
    }
}
