using EShopFanerum.AuthService.Helpers.Extensions;
using EShopFanerum.AuthService.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopFanerum.AuthService.Controllers;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("Index", User?.Identity?.Name); 
    }
}