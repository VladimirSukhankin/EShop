namespace EShopFanerum.Domain.Entites.Auth;

public class RefreshToken
{
    public int Id { get; set; }
        
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = null!;
    public DateTime? Revoked { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }

    public long UserId { get; set; }

    public User? User { get; set; }
}