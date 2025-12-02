using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

public class IdentityServerConfiguration
{
    public static List<TestUser> GetUsers()
    {
        var john = new TestUser()
        {
            SubjectId = "1",
            Username = "john",
            Password = "1111",
            Claims =
            {
                new Claim(type: "name"     /*JwtClaimTypes.Name*/,    value: "John Doe"),
                new Claim(type: "role"     /*JwtClaimTypes.Role*/,    value: "admin"),
                new Claim(type: "website"  /*JwtClaimTypes.WebSite*/, value: "https://medium.com/@iamprovidence"),
            }
        };

        return new List<TestUser> { john };
    }
    public static List<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
    {
        new ApiScope("api1", "Main API"),
        new ApiScope("electionapi", "Election Application API")

    };
    }
    public static List<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(), // new IdentityResource(name: "openId", userClaims new [] { "sub" })
            new IdentityResources.Profile(), // new IdentityResource(IdentityServerConstants.StandardScopes.Profile, new [] { "name", "email" ... } )
        };
    }
    public static List<Client> GetClients()
    {
        return new List<Client>
    {
        new Client
        {
            ClientId = "react-client",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,

            RedirectUris = { "http://localhost:5173/callback" },
            PostLogoutRedirectUris = { "http://localhost:5173/" },

            AllowedCorsOrigins = { "http://localhost:5173" },

            AllowedScopes =
            {
                "openid",
                "profile",
                "api1",
                "electionapi"
            },

            AllowAccessTokensViaBrowser = true,
            AccessTokenLifetime = 3600 // optional
        }
    };
    }
}