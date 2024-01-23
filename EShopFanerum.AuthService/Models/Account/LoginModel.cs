namespace EShopFanerum.AuthService.Models.Account;

public class LoginModel
{
    public string? Guid { get; set; }
    public string? Email { get; set; }
    public string? Code { get; set; }
    public DateTime Date { get; set; }
    public int Tries { get; set; } = 0;
}