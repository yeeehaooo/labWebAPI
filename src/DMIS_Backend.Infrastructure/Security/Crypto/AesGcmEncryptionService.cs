using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace DMIS_Backend.Infrastructure.Security.Crypto;

/// <summary>
/// AES-GCM 加密服務實作
/// </summary>
public class AesGcmEncryptionService : IAesGcmEncryptionService
{
  private readonly ILogger<AesGcmEncryptionService> _logger;

  public AesGcmEncryptionService(ILogger<AesGcmEncryptionService> logger)
  {
    _logger = logger;
  }

  public AesGcmEncryptionResult Encrypt(byte[] plainText, byte[] key, byte[]? aad = null, byte[]? nonce = null)
  {
    if (plainText == null || plainText.Length == 0)
    {
      throw new ArgumentException("Plain text cannot be null or empty", nameof(plainText));
    }

    if (key == null || key.Length != 32)
    {
      throw new ArgumentException("Key must be 32 bytes (256 bits)", nameof(key));
    }

    // GCM 模式使用 12 bytes (96 bits) 的 Nonce
    // 如果提供了 nonce，使用提供的；否則自動產生
    byte[] nonceToUse;
    if (nonce != null)
    {
      if (nonce.Length != 12)
      {
        throw new ArgumentException("Nonce must be 12 bytes (96 bits) for GCM mode", nameof(nonce));
      }
      nonceToUse = nonce;
    }
    else
    {
      nonceToUse = new byte[12];
      RandomNumberGenerator.Fill(nonceToUse);
    }

    // 建立認證標籤（16 bytes）
    var tag = new byte[16];

    // 使用 AES-GCM 加密
    var ciphertext = new byte[plainText.Length];
    using var aesGcm = new AesGcm(key);
    aesGcm.Encrypt(nonceToUse, plainText, ciphertext, tag, aad);

    return new AesGcmEncryptionResult
    {
      Nonce = nonceToUse,
      Ciphertext = ciphertext,
      Tag = tag
    };
  }

  public byte[] Decrypt(string encryptedData, byte[] key, byte[]? aad = null)
  {
    if (string.IsNullOrWhiteSpace(encryptedData))
    {
      throw new ArgumentException("Encrypted data cannot be null or empty", nameof(encryptedData));
    }

    if (key == null || key.Length != 32)
    {
      throw new ArgumentException("Key must be 32 bytes (256 bits)", nameof(key));
    }

    try
    {
      // 解析加密資料
      var result = AesGcmEncryptionResult.FromBase64String(encryptedData);

      // 使用 AES-GCM 解密（會自動驗證 Tag）
      var plaintext = new byte[result.Ciphertext.Length];
      using var aesGcm = new AesGcm(key);
      aesGcm.Decrypt(result.Nonce, result.Ciphertext, result.Tag, plaintext, aad);

      return plaintext;
    }
    catch (CryptographicException ex)
    {
      _logger.LogError(ex, "AES-GCM decryption failed - Tag verification failed");
      throw new CryptographicException("Decryption failed. Tag verification failed. Data may have been tampered.", ex);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "AES-GCM decryption failed");
      throw new CryptographicException("Decryption failed", ex);
    }
  }
}
