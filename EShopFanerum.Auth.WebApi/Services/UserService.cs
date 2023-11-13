using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using EShopFanerum.Auth.WebApi.Helpers;
using EShopFanerum.Auth.WebApi.Models;
using EShopFanerum.Auth.WebApi.Services.Interfaces;
using EShopFanerum.Domain.Entites;
using EShopFanerum.Domain.Repositories.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EShopFanerum.Auth.WebApi.Services;

public class UserService : IUserService
{
    private readonly AppSettings _appSettings;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(
        IOptions<AppSettings> appSettings,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _appSettings = appSettings.Value;
    }

    public async Task<bool> Create(RegisterModel newUser, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(newUser.Password))
            throw new WbApiException("Пароль пуст");

        if (await _userRepository.GetUserByLogin(newUser.Username, cancellationToken) != null)
            throw new WbApiException("Username \"" + newUser.Username + "\" существует в БД");

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(newUser.Password, out passwordHash, out passwordSalt);
        var user = _mapper.Map<User>(newUser);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        await _userRepository.CreateUser(user, cancellationToken);
        return true;
    }

    public async Task<bool> Update(UpdateModel updateUser, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserById(updateUser.Id, cancellationToken);

        if (user == null)
            throw new WebException("User not found");

        if (!string.IsNullOrWhiteSpace(updateUser.Username) && updateUser.Username != user.Username)
        {
            if (await _userRepository.GetUserByLogin(updateUser.Username, cancellationToken)!=null)
                throw new WebException("Username " + updateUser.Username + " is already taken");

            user.Username = updateUser.Username;
        }

        if (!string.IsNullOrWhiteSpace(updateUser.FirstName))
            user.FirstName = updateUser.FirstName;

        if (!string.IsNullOrWhiteSpace(updateUser.LastName))
            user.LastName = updateUser.LastName;

        if (!string.IsNullOrWhiteSpace(updateUser.Password))
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(updateUser.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        }

        await _userRepository.UpdateUser(user, cancellationToken);
        return true;
    }

    public Task<bool> Delete(long id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByLogin(model.Username, cancellationToken);
        if (user == null)
        {
            throw new WbApiException("Пользователь не найден");
        }

        if (!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new WbApiException("Пароль не верен");
        }
        
        var jwtToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken(ipAddress);

        user.RefreshTokens.Add(refreshToken);
        await _userRepository.UpdateUser(user, cancellationToken);

        return new AuthenticateResponse(_mapper.Map<UserModel>(user), jwtToken, refreshToken.Token);
    }

    public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByRefreshToken(token, cancellationToken);

        if (user == null)
        {
            return null;
        }

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.Revoked.HasValue && !(DateTime.UtcNow >= refreshToken.Expires))
        {
            return null;
        }

        var newRefreshToken = GenerateRefreshToken(ipAddress);
        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReplacedByToken = newRefreshToken.Token;
        user.RefreshTokens.Add(newRefreshToken);

        await _userRepository.UpdateUser(user, cancellationToken);

        var jwtToken = GenerateJwtToken(user);

        return new AuthenticateResponse(_mapper.Map<UserModel>(user), jwtToken, newRefreshToken.Token);
    }

    public async Task<bool> RevokeToken(string token, string ipAddress, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByRefreshToken(token, cancellationToken);

        if (user == null)
        {
            return false;
        }

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.Revoked.HasValue && !(DateTime.UtcNow >= refreshToken.Expires))
        {
            return false;
        }

        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        await _userRepository.UpdateUser(user, cancellationToken);

        return true;
    }

    public async Task<IEnumerable<UserModel>> GetAll(CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<UserModel>>(await _userRepository.GetUsers(cancellationToken));
    }

    public async Task<UserModel> GetById(int id, CancellationToken cancellationToken)
    {
        return _mapper.Map<UserModel>(await _userRepository.GetUserById(id, cancellationToken));
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[64];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (password == null) throw new ArgumentNullException("password");
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (password == null) throw new ArgumentNullException("password");
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
        if (storedHash.Length != 64)
            throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
        if (storedSalt.Length != 128)
            throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

        using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return !computedHash.Where((t, i) => t != storedHash[i]).Any();
    }
}