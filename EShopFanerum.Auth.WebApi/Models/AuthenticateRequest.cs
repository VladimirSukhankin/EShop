namespace EShopFanerum.Auth.WebApi.Models;

public class AuthenticateRequest
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}