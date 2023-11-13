using EShopFanerum.Auth.WebApi.Models;

namespace EShopFanerum.Auth.WebApi.Services.Interfaces;

public interface IUserService
{
    Task<bool> Create(RegisterModel newUser, CancellationToken cancellationToken);

    Task<bool> Update(UpdateModel updateUser, CancellationToken cancellationToken);

    Task<bool> Delete(long id, CancellationToken cancellationToken);
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress,
        CancellationToken cancellationToken);

    Task<AuthenticateResponse?> RefreshToken(string token, string ipAddress, CancellationToken cancellationToken);
    Task<bool> RevokeToken(string token, string ipAddress, CancellationToken cancellationToken);
    Task<IEnumerable<UserModel>> GetAll(CancellationToken cancellationToken);
    Task<UserModel> GetById(int id, CancellationToken cancellationToken);
}