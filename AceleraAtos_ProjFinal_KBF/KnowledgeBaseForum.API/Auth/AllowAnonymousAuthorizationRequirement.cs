using Microsoft.AspNetCore.Authorization;

namespace KnowledgeBaseForum.API.Auth
{
    public class AllowAnonymousAuthorizationRequirement :
        AuthorizationHandler<AllowAnonymousAuthorizationRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AllowAnonymousAuthorizationRequirement requirement)
        {
            if (context.User?.Identity == null || !context.User.Identities.Any(i => i.IsAuthenticated))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
