using EShopFanerum.Domain.Entites;
using Newtonsoft.Json;

namespace EShopFanerum.Auth.WebApi.Models;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore]
    public string RefreshToken { get; set; }

    public AuthenticateResponse(UserModel user, string jwtToken, string refreshToken)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}