using EShopFanerum.Domain.Entites;
using EShopFanerum.Domain.Entites.Auth;

namespace EShopFanerum.Domain.Repositories.Auth;

public interface IUserRepository
{
    Task CreateUser(User user, CancellationToken cancellationToken);
    Task UpdateUser(User newUser, CancellationToken cancellationToken);
    Task DeleteUser(long id, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken);
    Task<User?> GetUserByRefreshToken(string token, CancellationToken cancellationToken);
    Task<User?> GetUserByLoginAndPassword(string login, byte[] passwordHash, CancellationToken cancellationToken);
    Task<User?> GetUserById(long id, CancellationToken cancellationToken);
    Task<User?> GetUserByLogin(string login, CancellationToken cancellationToken);
}