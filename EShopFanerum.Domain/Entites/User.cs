using System.Text.Json.Serialization;

namespace EShopFanerum.Domain.Entites;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    
    public List<RefreshToken> RefreshTokens { get; set; } = null!;
}