using EShopFanerum.Domain.Entites;
using EShopFanerum.Domain.Entites.Auth;
using EShopFanerum.Domain.Repositories.Auth;
using EShopFanerum.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EShopFanerum.Persistance.Repositories.Auth;

public class UserRepository : IUserRepository
{
    private readonly AuthContext _authContext;

    public UserRepository(AuthContext authContext)
    {
        _authContext = authContext;
    }

    public async Task DeleteUser(long id, CancellationToken cancellationToken)
    {
        var user = await _authContext.Users.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (user == null)
        {
            throw new ArgumentNullException();
        }

        _authContext.Users.Remove(user);
        await _authContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken)
    {
        return await _authContext.Users.ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByRefreshToken(string token, CancellationToken cancellationToken)
    {
        return await _authContext.Users.Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.RefreshTokens.Any(rt => rt.Token == token), cancellationToken);
    }

    public async Task<User?> GetUserByLoginAndPassword(string login, byte[] passwordHash, CancellationToken cancellationToken)
    {
        return await _authContext.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(x => x.Username == login && x.PasswordHash == passwordHash, cancellationToken);
    }

    public async Task<User?> GetUserById(long id, CancellationToken cancellationToken)
    {
        return await _authContext.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<User?> GetUserByLogin(string login, CancellationToken cancellationToken)
    {
        return await _authContext.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(x => x.Username == login, cancellationToken);
    }

    public async Task CreateUser(User user, CancellationToken cancellationToken)
    {
        _authContext.Users.Add(user);
        await _authContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUser(User newUser,  CancellationToken cancellationToken)
    {
        try
        {
            _authContext.Users.Update(newUser);
            await _authContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            //пользовательский эксепшн
            throw;
        }
    }
}