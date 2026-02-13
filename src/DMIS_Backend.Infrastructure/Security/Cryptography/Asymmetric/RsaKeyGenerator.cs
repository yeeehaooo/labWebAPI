using System.Security.Cryptography;

namespace DMIS_Backend.Infrastructure.Security.Cryptography.Asymmetric;

/// <summary>
/// RSA 金鑰產生器工具類
/// 用於產生 RSA 非對稱加密所需的公鑰和私鑰
/// </summary>
public static class RsaKeyGenerator
{
  /// <summary>
  /// 產生 RSA 金鑰對
  /// </summary>
  /// <param name="keySize">金鑰長度（建議：2048 或 4096 bits）</param>
  /// <returns>包含公鑰和私鑰的元組（Base64 編碼）</returns>
  public static (string PublicKey, string PrivateKey) GenerateKeyPair(int keySize = 2048)
  {
    if (keySize < 1024 || keySize > 16384)
    {
      throw new ArgumentException("Key size must be between 1024 and 16384 bits.", nameof(keySize));
    }

    using var rsa = RSA.Create(keySize);
    var publicKeyBytes = rsa.ExportRSAPublicKey();
    var privateKeyBytes = rsa.ExportRSAPrivateKey();

    return (
      Convert.ToBase64String(publicKeyBytes),
      Convert.ToBase64String(privateKeyBytes)
    );
  }

  /// <summary>
  /// 產生 2048 bits RSA 金鑰對（推薦用於一般應用）
  /// </summary>
  public static (string PublicKey, string PrivateKey) GenerateKeyPair2048()
  {
    return GenerateKeyPair(2048);
  }

  /// <summary>
  /// 產生 4096 bits RSA 金鑰對（推薦用於高安全性需求）
  /// </summary>
  public static (string PublicKey, string PrivateKey) GenerateKeyPair4096()
  {
    return GenerateKeyPair(4096);
  }

  /// <summary>
  /// 從現有的 RSA 實例匯出金鑰對
  /// </summary>
  public static (string PublicKey, string PrivateKey) ExportKeyPair(RSA rsa)
  {
    if (rsa == null)
    {
      throw new ArgumentNullException(nameof(rsa));
    }

    var publicKeyBytes = rsa.ExportRSAPublicKey();
    var privateKeyBytes = rsa.ExportRSAPrivateKey();

    return (
      Convert.ToBase64String(publicKeyBytes),
      Convert.ToBase64String(privateKeyBytes)
    );
  }
}
