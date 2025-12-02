using System.Security.Cryptography;
using System.Text;

public static class PKCE
{
    public static (string Verifier, string Challenge) Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        var verifier = Base64UrlEncode(bytes);

        using var sha256 = SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(verifier));

        var challenge = Base64UrlEncode(challengeBytes);
        return (verifier, challenge);
    }

    private static string Base64UrlEncode(byte[] arg)
    {
        return Convert.ToBase64String(arg)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }
}
