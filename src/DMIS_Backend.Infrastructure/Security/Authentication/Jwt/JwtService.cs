using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DMIS_Backend.Infrastructure.Security.Authentication.Jwt;

/// <summary>
/// JWT 服務實作
/// </summary>
public class JwtService : IJwtService
{
  private readonly IConfiguration _configuration;
  private readonly KeyProvider _keyProvider;

  public JwtService(IConfiguration configuration, KeyProvider keyProvider)
  {
    _configuration = configuration;
    _keyProvider = keyProvider;
  }

  /// <summary>
  /// 產生 JWT Token
  /// </summary>
  public string GenerateToken(IEnumerable<Claim> claims, int expirationMinutes = 60)
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyProvider.GetJwtSecretKey()));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  /// <summary>
  /// 驗證 JWT Token
  /// </summary>
  public ClaimsPrincipal? ValidateToken(string token)
  {
    try
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_keyProvider.GetJwtSecretKey());

      var validationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = _configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = _configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
      return principal;
    }
    catch
    {
      return null;
    }
  }
}
