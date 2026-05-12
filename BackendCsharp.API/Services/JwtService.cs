using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendCsharp.API.Entities;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(UserEntity user)
    {
        var keyString = _config["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(keyString))
            throw new InvalidOperationException("JWT Key não configurada.");

        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var expiresInMinutesRaw = _config["Jwt:ExpiresInMinutes"];

        if (string.IsNullOrWhiteSpace(expiresInMinutesRaw))
            throw new InvalidOperationException("Jwt:ExpiresInMinutes não configurado.");

        if (!double.TryParse(expiresInMinutesRaw, out var expiresInMinutes))
            throw new InvalidOperationException("Jwt:ExpiresInMinutes inválido.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(
    JwtRegisteredClaimNames.Email,
    user.Email ?? string.Empty,
    ClaimValueTypes.String
),
            new Claim(JwtRegisteredClaimNames.Name, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}