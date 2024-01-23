namespace EShopFanerum.AuthService.Models.Account;

public class LoginVm
{
    public string? ReturnUrl { get; set; }
    public string? Email { get; set; }
    public string? Code { get; set; }
    public bool? EnableLocalLogin { get; set; }
    public string? Err { get; set; }
}
