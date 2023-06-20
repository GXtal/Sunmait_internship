using Microsoft.IdentityModel.Tokens;
using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Web.AuthorizationData;

namespace Web.Authorization;

public class TokenManager
{
    private readonly WebApplicationBuilder _applicationBuilder;
    private static readonly TimeSpan tokenLifeTime = TimeSpan.FromMinutes(20);

    public TokenManager(WebApplicationBuilder applicationBuilder)
    {
        _applicationBuilder = applicationBuilder;
    }
    public string GenerateToken(User user)
    {
        var config = _applicationBuilder.Configuration;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(config[ConfigurationPaths.Key]!);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(CustomClaimNames.UserId, user.Id.ToString()),
            new Claim(CustomClaimNames.RoleId, user.RoleId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(tokenLifeTime),
            Issuer = config[ConfigurationPaths.Issuer],
            Audience = config[ConfigurationPaths.Audience],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
