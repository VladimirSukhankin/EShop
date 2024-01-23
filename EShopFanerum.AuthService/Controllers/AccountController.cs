using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using EShopFanerum.AuthService.Helpers.Extensions;
using EShopFanerum.AuthService.Logic;
using EShopFanerum.AuthService.Models;
using EShopFanerum.AuthService.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EShopFanerum.AuthService.Controllers;

/// <summary>
/// 
/// </summary>
[SecurityHeaders]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IIdentityServerInteractionService _serverInteraction;
    private readonly IEventService _eventService;
    private readonly IClientStore _clientStore;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly ILogger<AccountController> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;

    private static TimeSpan? _cookieAge;
    private static string? _domain;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IIdentityServerInteractionService serverInteraction,
        IEventService eventService,
        IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider,
        ILogger<AccountController> logger,
        IMemoryCache memoryCache,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _serverInteraction = serverInteraction;
        _eventService = eventService;
        _clientStore = clientStore;
        _schemeProvider = schemeProvider;
        _logger = logger;
        _memoryCache = memoryCache;
        _configuration = configuration;
    }

    /// <summary>
    /// Первичная форма логина
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Login([FromQuery] LoginVm? request)
    {
        if (User.Identity?.IsAuthenticated is true)
        {
            if (request?.ReturnUrl?.Length is not > 5) return RedirectToAction("index", "home");
            Response.StatusCode = 200;
            Response.Headers["Location"] = "";
            return View("Redirect",
                new RedirectVm {RedirectUrl = request.ReturnUrl});
        }

        Request.Cookies.TryGetValue(Consts.OfflineCookie, out var cookie);
        if (!Guid.TryParse(cookie, out var guid))
        {
            cookie = Guid.NewGuid().ToString();
        }

        Response.Cookies.Append(Consts.OfflineCookie, cookie, new CookieOptions
        {
            //Domain = Domain,
            HttpOnly = true,
            Secure = true,
            Path = "/",
            MaxAge = _cookieAge
        });

        return View("Login", await BuildLoginViewModelAsync(request ?? new LoginVm()));
    }

    /// <summary>
    /// Форма LogOut.
    /// </summary>
    public async Task<IActionResult> Logout(string? logoutId, bool? showLogout)
    {
        var showLogoutPrompt = showLogout ?? LogoutOptions.ShowLogoutPrompt;

        if (User?.Identity?.IsAuthenticated is not true)
        {
            showLogoutPrompt = false;
        }
        else
        {
            var context = await _serverInteraction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                showLogoutPrompt = false;
            }
        }

        var req = new LogoutVm {LogoutId = logoutId};

        if (showLogoutPrompt == false)
        {
            return RedirectToAction("logoutdo", "account", req);
        }

        return View("Logout", req);
    }

    /// <summary>
    /// LogOut пользователя.
    /// </summary>
    public async Task<IActionResult> LogoutDo(LogoutVm request)
    {
        if (User?.Identity?.IsAuthenticated is not true) return View("LoggedOut", new LogoutVm());

        request.LogoutId ??= await _serverInteraction.CreateLogoutContextAsync();
        var logout = await _serverInteraction.GetLogoutContextAsync(request.LogoutId);

        request.AutomaticRedirectAfterSignOut ??= LogoutOptions.AutomaticRedirectAfterSignOut;
        request.PostLogoutRedirectUri ??= logout?.PostLogoutRedirectUri;
        request.ClientName = string.IsNullOrEmpty(logout?.ClientName)
            ? $"\"Приложение {logout?.ClientId}\"."
            : $"\"{logout?.ClientName}\".";
        request.SignOutIframeUrl ??= logout?.SignOutIFrameUrl;

        _logger.LogInformation(
            $"-OIDC- Logged out, User:{User?.Identity.Name ?? User?.GetSubjectId()}, Cli:{request.ClientName}");

        await _signInManager.SignOutAsync();
        await _eventService.RaiseAsync(new UserLogoutSuccessEvent(User?.GetSubjectId(), User?.Identity.Name));

        return View("LoggedOut", request);
    }

    /// <summary>
    /// Ввод кода
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EmailDo(LoginVm req, CancellationToken stoppingToken)
    {
        var context = await _serverInteraction.GetAuthorizationContextAsync(req.ReturnUrl);
        if (User.Identity?.IsAuthenticated is true)
        {
            if (req.ReturnUrl?.Length is not > 5) return RedirectToAction("index", "home");
            Response.StatusCode = 200;
            Response.Headers["Location"] = "";
            return View("Redirect", new RedirectVm {RedirectUrl = req.ReturnUrl});
        }

        if (!Request.Cookies.TryGetValue(Consts.OfflineCookie, out var cookie) || !Guid.TryParse(cookie, out var guid))
        {
            if (context != null)
                await _serverInteraction.DenyAuthorizationAsync(context, AuthorizationError.LoginRequired);
            return RedirectToAction("login", "account", await BuildLoginViewModelAsync(req));
        }

        if (!Request.CheckIp(_memoryCache))
        {
            if (context != null)
                await _serverInteraction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
            return View("TooMuch");
        }


        var user = await _userManager.FindByNameAsync(req.Email);

        _memoryCache.TryGetValue($"{Consts.LoginGcode}{guid}", out LoginModel? lmg);
        _memoryCache.TryGetValue($"{Consts.LoginFcode}{req.Email}", out LoginModel? lmf);
        var dt = DateTime.Now;

        var dtt = DateTime.Now;

        var loginModelGCode = new LoginModel
        {
            Guid = guid.ToString(),
            Email = req.Email,
            Date = dtt,
            Tries = 0
        };

        var loginModelFCode = new LoginModel
        {
            Guid = guid.ToString(),
            Email = req.Email,
            Date = dtt,
            Tries = 0
        };

        _memoryCache.Set($"{Consts.LoginGcode}{guid}", loginModelGCode, TimeSpan.FromSeconds(1800));
        _memoryCache.Set($"{Consts.LoginFcode}{req.Email}", loginModelFCode, TimeSpan.FromSeconds(1800));

        Response.Cookies.Append(Consts.OfflineCookie, cookie, new CookieOptions
        {
            //Domain = Domain,
            HttpOnly = true,
            Secure = true,
            Path = "/",
            MaxAge = _cookieAge
        });

        //Теперь логинимся .....
        _memoryCache.Remove($"{Consts.LoginGcode}{guid}");
        _memoryCache.Remove($"{Consts.LoginFcode}{req.Email}");
        Response.Cookies.Delete(Consts.OfflineCookie);
        try
        {
            if (user is null)
            {
                await _userManager.CreateAsync(new IdentityUser()
                {
                    UserName = req.Email,
                    NormalizedUserName = req.Email,
                }, Guid.NewGuid().ToString().Sha256());
                user = await _userManager.FindByNameAsync(req.Email);
                if (user != null) await _userManager.AddToRoleAsync(user, "user");
            }

            if (user == null)
            {
                if (context != null)
                    await _serverInteraction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                req.Err = "Произошло что-то непонятное, попробуйте ещё раз...";
                return RedirectToAction("login", "account", await BuildLoginViewModelAsync(req));
            }

            await _signInManager.SignInAsync(user, true);

            await _userManager.UpdateAsync(user);
            await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName,
                clientId: context?.Client.ClientId));
            _logger.LogInformation($"-OIDC- Events:{user.UserName}, redirecting to:{req.ReturnUrl}");

            if (context != null)
            {
                Response.StatusCode = 200;
                Response.Headers["Location"] = "";
                return View("Redirect", new RedirectVm {RedirectUrl = req.ReturnUrl});
            }

            if (Url.IsLocalUrl(req.ReturnUrl))
            {
                return Redirect(req.ReturnUrl);
            }

            return RedirectToAction("index", "home");
        }
        catch (Exception ex)
        {
            req.Err = "Произошло что-то непонятное, попробуйте ещё раз...";
            try
            {
                var p1 = _configuration.GetValue<string>("TgAlerter:KeyFilePath");
                var p2 = _configuration.GetValue<string>("TgAlerter:ChatId");
                if (p1?.Length is > 0 && p2?.Length is > 0)
                {
                    var p1R = await System.IO.File.ReadAllTextAsync(p1, stoppingToken);
                    if (p1R?.Length is > 0)
                    {
                        //await Alerter.SendAsync("oauth login", ex.Message, p1r, p2, stoppingToken);
                    }
                }
            }
            catch
            {
                // ignored
            }

            return RedirectToAction("login", "account", await BuildLoginViewModelAsync(req));
        }
    }

    #region Вспомогательные методы

    private async Task<LoginVm> BuildLoginViewModelAsync(LoginVm model)
    {
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        vm.Email = model.Email;
        vm.Code = null;
        vm.Err = model.Err;
        return vm;
    }

    private async Task<LoginVm> BuildLoginViewModelAsync(string? returnUrl)
    {
        var context = await _serverInteraction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;
            var vm = new LoginVm
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl
            };
            return vm;
        }

        var allowLocal = true;
        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;
            }
        }

        return new LoginVm
        {
            EnableLocalLogin = allowLocal,
            ReturnUrl = returnUrl,
        };
    }

    #endregion
}