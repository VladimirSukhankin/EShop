namespace EShopFanerum.Domain.Entites.Auth;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    
    public List<RefreshToken> RefreshTokens { get; set; } = null!;
}