using EShopFanerum.Domain.Entites;
using EShopFanerum.Domain.Entites.Auth;

namespace EShopFanerum.Domain.Repositories.Auth;

public interface IRefreshTokenRepository
{
    Task<bool> CreateRefreshToken(RefreshToken token, CancellationToken cancellationToken);
}