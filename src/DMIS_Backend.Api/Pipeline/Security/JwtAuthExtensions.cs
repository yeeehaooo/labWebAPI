using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DMIS_Backend.Api.Pipeline.Security;

/// <summary>
/// JWT 認證擴充方法
/// 配置 JWT Bearer 認證服務
/// </summary>
public static class AuthExtensions
{
  /// <summary>
  /// 註冊 JWT Bearer 認證服務
  /// </summary>
  /// <param name="services">服務集合</param>
  /// <param name="config">配置物件</param>
  /// <returns>服務集合</returns>
  public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
  {
    var issuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer configuration is required");
    var audience = config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience configuration is required");
    var key = config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key configuration is required");

    services.AddAuthentication("Bearer")
      .AddJwtBearer("Bearer", options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = issuer,
          ValidAudience = audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
      });

    return services;
  }
}
