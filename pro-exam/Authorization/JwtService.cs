using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace pro_exam.Authorization;
public class JwtService
{

    public static string DecodeJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        if (jsonToken == null)
            throw new ArgumentException("Invalid JWT token");

        var payload = jsonToken.Claims
            .Select(c => new { c.Type, c.Value })
            .ToDictionary(c => c.Type, c => c.Value);

        return JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
    }



}
