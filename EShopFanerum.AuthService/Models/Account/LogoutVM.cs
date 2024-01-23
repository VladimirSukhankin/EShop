namespace EShopFanerum.AuthService.Models.Account;

public class LogoutVm
{
    public string? UserName { get; set; }
    public string? LogoutId { get; set; }
    public string? PostLogoutRedirectUri { get; set; }
    public string? ClientName { get; set; }
    public string? SignOutIframeUrl { get; set; }
    public bool? AutomaticRedirectAfterSignOut { get; set; }
}