using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BackendCsharp.API.Entities;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(UserEntity user)
    {
        try{
        var keyString = _config["Jwt:Key"];
            Console.WriteLine(keyString);
        if (string.IsNullOrEmpty(keyString))
            throw new Exception("JWT Key não configurada");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(keyString)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])
            ),
            signingCredentials: creds
        );

            return new JwtSecurityTokenHandler().WriteToken(token); }catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gerar token JWT: {ex.Message}");
            return null;
        }
    }
}