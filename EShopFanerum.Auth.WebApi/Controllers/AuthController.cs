using EShopFanerum.Auth.WebApi.Models;
using EShopFanerum.Auth.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.Auth.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model, CancellationToken cancellationToken)
    {
        return Ok(await _userService.Create(model, cancellationToken));
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model,
        CancellationToken cancellationToken)
    {
        var response = await _userService.Authenticate(model, IpAddress(), cancellationToken);

        if (response == null)
            return BadRequest(new {message = "Username or password is incorrect"});

        SetTokenCookie(response.RefreshToken);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshToken(refreshToken!, IpAddress(), cancellationToken);

        if (response == null)
            return Unauthorized(new {message = "Invalid token"});

        SetTokenCookie(response.RefreshToken);

        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateModel model, CancellationToken cancellationToken)
    {
        return Ok(await _userService.Update(model, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        return Ok(await _userService.Delete(id, cancellationToken));
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model,
        CancellationToken cancellationToken)
    {
        // accept token from request body or cookie
        var token = model.Token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(token))
            return BadRequest(new {message = "Token is required"});

        var response = await _userService.RevokeToken(token, IpAddress(), cancellationToken);

        if (!response)
            return NotFound(new {message = "Token not found"});

        return Ok(new {message = "Token revoked"});
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAll(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetById(id, cancellationToken);

        return Ok(user);
    }

    [HttpGet("{id}/refresh-tokens")]
    public async Task<IActionResult> GetRefreshTokens(int id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetById(id, cancellationToken);
       
        return Ok(user.RefreshTokens);
    }

    // helper methods

    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }

    private string IpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"]!;
        return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
    }
}