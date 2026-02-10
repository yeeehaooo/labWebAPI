using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace DMIS_Backend.Infrastructure.Security.Integrity;

/// <summary>
/// HMAC 簽名服務實作
/// </summary>
public class HmacSignatureService : IHmacSignatureService
{
  private readonly ILogger<HmacSignatureService> _logger;

  public HmacSignatureService(ILogger<HmacSignatureService> logger)
  {
    _logger = logger;
  }

  public string GenerateSignature(byte[] key, string data)
  {
    if (key == null || key.Length == 0)
    {
      throw new ArgumentException("Key cannot be null or empty", nameof(key));
    }

    if (string.IsNullOrWhiteSpace(data))
    {
      throw new ArgumentException("Data cannot be null or empty", nameof(data));
    }

    using var hmac = new HMACSHA256(key);
    var dataBytes = Encoding.UTF8.GetBytes(data);
    var hashBytes = hmac.ComputeHash(dataBytes);
    return Convert.ToBase64String(hashBytes);
  }

  public bool VerifySignature(byte[] key, string data, string signature)
  {
    if (key == null || key.Length == 0)
    {
      return false;
    }

    if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(signature))
    {
      return false;
    }

    try
    {
      var expectedSignature = GenerateSignature(key, data);
      var expectedBytes = Convert.FromBase64String(expectedSignature);
      var providedBytes = Convert.FromBase64String(signature);

      // 使用時間安全的比較（防止時間攻擊）
      return CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes);
    }
    catch (Exception ex)
    {
      _logger.LogWarning(ex, "HMAC signature verification failed");
      return false;
    }
  }
}
