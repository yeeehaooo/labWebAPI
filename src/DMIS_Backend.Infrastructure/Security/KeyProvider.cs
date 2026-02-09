using Microsoft.Extensions.Configuration;

namespace DMIS_Backend.Infrastructure.Security;

/// <summary>
/// 金鑰提供者
/// 統一管理加密金鑰和密鑰
/// </summary>
public class KeyProvider
{
  private readonly IConfiguration _configuration;

  public KeyProvider(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  /// <summary>
  /// 取得 JWT Secret Key
  /// </summary>
  public string GetJwtSecretKey()
  {
    return _configuration["Jwt:SecretKey"]
        ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
  }

  /// <summary>
  /// 取得加密金鑰
  /// </summary>
  public string GetEncryptionKey()
  {
    return _configuration["Encryption:Key"]
        ?? throw new InvalidOperationException("Encryption Key is not configured.");
  }
}
