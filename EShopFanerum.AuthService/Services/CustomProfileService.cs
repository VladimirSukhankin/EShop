using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace EShopFanerum.AuthService.Services;

/// <summary>
/// Сервис для переопределения утверждений(claim) пользователя.
/// </summary>
public class CustomProfileService : ProfileService<IdentityUser>
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="CustomProfileService"/>.
    /// </summary>
    public CustomProfileService(
        UserManager<IdentityUser> userManager,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory) : base(userManager, claimsFactory)
    {
    }

    /// <inheritdoc />
    protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, IdentityUser user)
    {
        var principal = await GetUserClaimsAsync(user);
        if(principal?.Claims.Any() is true)
        {
            context.AddRequestedClaims(principal.Claims);
            context.IssuedClaims.RemoveAll(x => x.Type == JwtClaimTypes.PreferredUserName);
            context.IssuedClaims.Add(new System.Security.Claims.Claim(JwtClaimTypes.PreferredUserName, user.UserName ?? user.PhoneNumber ?? user.UserName ?? "-"));

            if(!context.IssuedClaims.Any(x => x.Type == JwtClaimTypes.Name))
            {
                context.IssuedClaims.Add(new System.Security.Claims.Claim(JwtClaimTypes.Name, user.UserName ?? "-"));
            }

            if (!context.IssuedClaims.Any(x => x.Type == JwtClaimTypes.Role))
            {				
                foreach(var claim in principal.Claims.Where(x => x.Type == JwtClaimTypes.Role))
                {
                    context.IssuedClaims.Add(new System.Security.Claims.Claim(JwtClaimTypes.Role, claim.Value));
                }
            }

            if (!context.IssuedClaims.Any(x => x.Type == "realty_role"))
            {
                foreach (var claim in principal.Claims.Where(x => x.Type == "realty_role"))
                {
                    context.IssuedClaims.Add(new System.Security.Claims.Claim("realty_role", claim.Value));
                }
            }

        }
    }
}