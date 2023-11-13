namespace EShopFanerum.Auth.WebApi.Models;

public class UserModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    
    public List<RefreshTokenModel> RefreshTokens { get; set; }
}