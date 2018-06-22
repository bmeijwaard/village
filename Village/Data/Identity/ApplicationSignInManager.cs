using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Village.Common.Types;
using Village.Data.Domain;

namespace Village.Data.Identity
{
    public class ApplicationSignInManager : SignInManager<User>
    {

        public ApplicationSignInManager(UserManager<User> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<User>> logger,
            IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockoutOnFailure)
        {
            return await base.PasswordSignInAsync(userName, password, rememberMe, lockoutOnFailure);
        }

        public async Task SignInAsync(User user, bool isPersistent)
        {
            await base.SignInAsync(user, isPersistent);
        }

        public override bool IsSignedIn(ClaimsPrincipal user)
        {
            return base.IsSignedIn(user);
        }

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user)
        {
            var principal = await base.CreateUserPrincipalAsync(user);
            var isAdmin = await UserManager.IsInRoleAsync(user, Roles.Administrator.ToString());

            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimsStore.ADMINISTRATOR, isAdmin.ToString())
            });

            return principal;
        }
    }
}
