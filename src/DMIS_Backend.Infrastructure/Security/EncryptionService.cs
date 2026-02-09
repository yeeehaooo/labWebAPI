using System.Security.Cryptography;
using System.Text;

namespace DMIS_Backend.Infrastructure.Security;

/// <summary>
/// 加密服務
/// </summary>
public class EncryptionService
{
  private readonly KeyProvider _keyProvider;

  public EncryptionService(KeyProvider keyProvider)
  {
    _keyProvider = keyProvider;
  }

  /// <summary>
  /// AES 加密
  /// </summary>
  public string Encrypt(string plainText)
  {
    // TODO: 實作 AES 加密邏輯
    throw new NotImplementedException("EncryptionService.Encrypt needs to be implemented.");
  }

  /// <summary>
  /// AES 解密
  /// </summary>
  public string Decrypt(string cipherText)
  {
    // TODO: 實作 AES 解密邏輯
    throw new NotImplementedException("EncryptionService.Decrypt needs to be implemented.");
  }

  /// <summary>
  /// 雜湊（用於密碼等）
  /// </summary>
  public string Hash(string input)
  {
    using var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(input);
    var hash = sha256.ComputeHash(bytes);
    return Convert.ToBase64String(hash);
  }
}
