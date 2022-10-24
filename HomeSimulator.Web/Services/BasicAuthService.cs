namespace HomeSimulator.Services;

using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
public static class BasicAuthenticationScheme
{
    public const string DefaultScheme = "Basic";
}

public class BasicAuthenticationOption : AuthenticationSchemeOptions
{
    public string Realm { get; set; }
    public string UserName { get; set; }
    public string UserPwd { get; set; }
}

public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOption>
{
    private readonly BasicAuthenticationOption authOptions;
    private readonly ConfigService _configService;
    public BasicAuthenticationHandler(
        IOptionsMonitor<BasicAuthenticationOption> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ConfigService configService)
        : base(options, logger, encoder, clock)
    {
        authOptions = options.CurrentValue;
        _configService=configService;
    }

    /// <summary>
    /// 认证
    /// </summary>
    /// <returns></returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Missing Authorization Header");
        string username, password;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            username = credentials[0];
            password = credentials[1];
            var isValidUser = IsAuthorized(username, password);
            if (isValidUser == false)
            {
                return AuthenticateResult.Fail("Invalid username or password");
            }
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier,username),
                new Claim(ClaimTypes.Name,username),
            };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }

    /// <summary>
    /// 质询
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{Options.Realm}\"";
        await base.HandleChallengeAsync(properties);
    }

    /// <summary>
    /// 认证失败
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        await base.HandleForbiddenAsync(properties);
    }

    private bool IsAuthorized(string username, string password)
    {
        return _configService.ValidateUser(username,password);
    }
}

public class BasicAuthenticationMiddleware
{
     private readonly RequestDelegate _next;
     private readonly ILogger _logger;

     public BasicAuthenticationMiddleware(RequestDelegate next, ILoggerFactory LoggerFactory)
     {
        _next = next;
        _logger = LoggerFactory.CreateLogger<BasicAuthenticationMiddleware>();
     }
     public async Task Invoke(HttpContext httpContext, IAuthenticationService authenticationService)
     {
       var authenticated = await authenticationService.AuthenticateAsync(httpContext, BasicAuthenticationScheme.DefaultScheme);
       _logger.LogInformation("Access Status：" + authenticated.Succeeded);
       if (!authenticated.Succeeded)
       {
           await authenticationService.ChallengeAsync(httpContext, BasicAuthenticationScheme.DefaultScheme, new AuthenticationProperties { });
           return;
       }
       await _next(httpContext);
     }
}