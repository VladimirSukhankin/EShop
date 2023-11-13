using EShopFanerum.Domain.Entites;

namespace EShopFanerum.Domain.Repositories.Auth;

public interface IRefreshTokenRepository
{
    Task<bool> CreateRefreshToken(RefreshToken token, CancellationToken cancellationToken);
}