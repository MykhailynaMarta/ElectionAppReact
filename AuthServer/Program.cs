using System.Security.Claims;
using AuthServer.Services;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddTestUsers(IdentityServerConfiguration.GetUsers())
    .AddInMemoryClients(IdentityServerConfiguration.GetClients())
    .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
    .AddInMemoryApiScopes(IdentityServerConfiguration.GetApiScopes())
    .AddProfileService<AutoProfileService>();

builder.Services.AddAuthentication("cookies")
    .AddCookie("cookies");

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.Use(async (ctx, next) =>
{
    var isAuthPage = ctx.Request.Path.StartsWithSegments("/connect/authorize");

    if (isAuthPage && !ctx.User.Identity.IsAuthenticated)
    {
        var claims = new[]
        {
            new Claim("sub", "user1"),
            new Claim("name", "Authed user")
        };

        var identity = new ClaimsIdentity(claims, "cookies");
        var principal = new ClaimsPrincipal(identity);

        await ctx.SignInAsync("cookies", principal);
    }

    await next();
});


app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
