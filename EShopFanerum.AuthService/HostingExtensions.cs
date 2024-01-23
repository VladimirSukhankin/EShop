using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using Duende.IdentityServer;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.Services;
using Duende.IdentityServer.EntityFramework.Stores;
using EShopFanerum.AuthService.Data;
using EShopFanerum.AuthService.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace EShopFanerum.AuthService;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        //const string dbSchema = "oauth";
        var config = builder.Configuration;
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        var migrationAssembly = typeof(HostingExtensions).GetTypeInfo().Assembly.GetName().Name;
        
        var kfStore = config?.GetValue<string>("SIGNING_CREDENTIALS_KEY_FILE_STORE");
        if (kfStore?.Length is > 0)
        {
            builder.Services
                .AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(kfStore));
        }
        
        builder.Services.AddDbContext<UsersDbContext>(options =>
        {
            options.UseNpgsql(connectionString, sqlOpts =>
            {
                sqlOpts.MigrationsAssembly(migrationAssembly);
            });
        });

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 7;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false; 
        })
        .AddEntityFrameworkStores<UsersDbContext>()
        .AddRoles<IdentityRole>()
        .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitScopesAsSpaceDelimitedStringInJwt = true;
                options.UserInteraction.LoginUrl = "/account/login";
                options.UserInteraction.LogoutUrl = "/account/logout";
                options.UserInteraction.ErrorUrl = "/errors/error";
                options.Csp.AddDeprecatedHeader = false;			
                options.Caching.ClientStoreExpiration =
                    TimeSpan.FromMinutes(config?.GetValue<int>("CACHING_CLIENTS_MINUTES") ?? 15);
                options.Caching.ResourceStoreExpiration =
                    TimeSpan.FromMinutes(config?.GetValue<int>("CACHING_RESOURCES_MINUTES") ?? 15);
                options.Caching.CorsExpiration =
                    TimeSpan.FromMinutes(config?.GetValue<int>("CACHING_CORS_MINUTES") ?? 15);
                options.ServerSideSessions.UserDisplayNameClaimType = JwtClaimTypes.PreferredUserName;
                options.ServerSideSessions.RemoveExpiredSessions = true;
                options.ServerSideSessions.ExpiredSessionsTriggerBackchannelLogout = true;
                options.KeyManagement.Enabled = true;
                if (kfStore?.Length is > 0) options.KeyManagement.KeyPath = kfStore;
                options.KeyManagement.SigningAlgorithms = new[]
                {
                    new SigningAlgorithmOptions(SecurityAlgorithms.RsaSha256),
                    new SigningAlgorithmOptions(SecurityAlgorithms.RsaSsaPssSha256),
                    new SigningAlgorithmOptions(SecurityAlgorithms.EcdsaSha256)
                };
            })
            .AddServerSideSessions()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(connectionString, sqlOpts =>
                {
                    sqlOpts.MigrationsAssembly(migrationAssembly);
                });
            })
            .AddConfigurationStoreCache()
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(connectionString, sqlOpts =>
                {
                    sqlOpts.MigrationsAssembly(migrationAssembly);
                });
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<IdentityUser>()
            .AddInMemoryCaching()
            .AddClientStoreCache<ClientStore>()
            .AddResourceStoreCache<ResourceStore>()
            .AddCorsPolicyCache<CorsPolicyService>()
            .AddJwtBearerClientAuthentication()
            .AddProfileService<CustomProfileService>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = ".Villagio.OAuth";
            options.Cookie.HttpOnly = true;
        });
        
        builder.Services
        .AddAuthentication()
        .AddLocalApi();

    builder.Services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
    });

    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.MaxAge = TimeSpan.FromDays(181);
    });
    builder.Services.AddAuthorization();

    builder.Services.AddMemoryCache();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("cors-any", policy =>
        {
            policy
                .SetIsOriginAllowed(origin =>
                {
                    var host = (new Uri(origin)).Host.ToLower();
                    return host.Contains("villagio.tech") || host == "localhost";
                })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

    builder.Services.AddControllersWithViews().AddJsonOptions(options => {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });
   
    return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/errors/error500");
            app.UseHsts();
        }
        
        app.UseSerilogRequestLogging();
        
        app.UseStatusCodePagesWithReExecute("/errors/error/{0}");
        app.UseHttpsRedirection();
        app.UseForwardedHeaders();
        app.UseStaticFiles(new StaticFileOptions()
        {
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=31536000");
            }
        });
        
        app.UseRouting();
        app.UseCors("cors-any");
        app.UseIdentityServer();
        app.UseAuthorization();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .RequireAuthorization();
        return app;
    }
    
}