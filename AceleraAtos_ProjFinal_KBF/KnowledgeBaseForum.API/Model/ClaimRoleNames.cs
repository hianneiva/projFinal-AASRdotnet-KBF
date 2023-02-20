using KnowledgeBaseForum.Commons.Utils;

namespace KnowledgeBaseForum.API.Model
{
    public class ClaimRoleNames
    {
        public static readonly IDictionary<int, string> USER_ROLE_NAMES = new Dictionary<int, string>()
        {
            { 1, Const.ROLE_NORMAL },
            { 2, Const.ROLE_ADMIN }
        };
    }
}
