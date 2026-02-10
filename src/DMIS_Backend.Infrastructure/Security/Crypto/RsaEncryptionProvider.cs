using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DMIS_Backend.Infrastructure.Security.Crypto;

/// <summary>
/// RSA 非對稱加密/解密提供者實作
/// 支援 RSA 加密、解密、數位簽章和驗證
/// </summary>
public class RsaEncryptionProvider : IRsaEncryptionProvider, IDisposable
{
  private readonly RSA _rsa;
  private readonly bool _disposeRsa;

  /// <summary>
  /// 從設定檔載入 RSA 金鑰
  /// </summary>
  public RsaEncryptionProvider(IConfiguration configuration)
  {
    var publicKeyBase64 = configuration["RsaEncryption:PublicKey"];
    var privateKeyBase64 = configuration["RsaEncryption:PrivateKey"];

    if (string.IsNullOrWhiteSpace(publicKeyBase64))
    {
      throw new InvalidOperationException("RsaEncryption:PublicKey is not configured.");
    }

    _rsa = RSA.Create();
    _disposeRsa = true;

    // 載入公鑰
    var publicKeyBytes = Convert.FromBase64String(publicKeyBase64);
    _rsa.ImportRSAPublicKey(publicKeyBytes, out _);

    // 如果有私鑰，載入私鑰（用於解密和簽章）
    if (!string.IsNullOrWhiteSpace(privateKeyBase64))
    {
      var privateKeyBytes = Convert.FromBase64String(privateKeyBase64);
      _rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
    }
  }

  /// <summary>
  /// 使用現有的 RSA 實例（用於測試或特殊場景）
  /// </summary>
  public RsaEncryptionProvider(RSA rsa, bool disposeRsa = false)
  {
    _rsa = rsa ?? throw new ArgumentNullException(nameof(rsa));
    _disposeRsa = disposeRsa;
  }

  /// <summary>
  /// 使用公鑰加密資料
  /// RSA 加密有資料長度限制，較大資料需分段加密或使用混合加密
  /// </summary>
  public string Encrypt(string plainText)
  {
    if (string.IsNullOrWhiteSpace(plainText))
    {
      return plainText;
    }

    var plainBytes = Encoding.UTF8.GetBytes(plainText);
    var keySize = _rsa.KeySize;
    var maxBlockSize = (keySize / 8) - 42; // RSA PKCS#1 v1.5 padding 需要 11 bytes，但實際可用空間更小

    // 如果資料小於最大區塊大小，直接加密
    if (plainBytes.Length <= maxBlockSize)
    {
      var encryptedBytes = _rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);
      return Convert.ToBase64String(encryptedBytes);
    }

    // 分段加密（適用於較大資料）
    using var msEncrypt = new MemoryStream();
    var offset = 0;

    while (offset < plainBytes.Length)
    {
      var blockSize = Math.Min(maxBlockSize, plainBytes.Length - offset);
      var block = new byte[blockSize];
      Array.Copy(plainBytes, offset, block, 0, blockSize);

      var encryptedBlock = _rsa.Encrypt(block, RSAEncryptionPadding.OaepSHA256);
      var lengthBytes = BitConverter.GetBytes(encryptedBlock.Length);
      msEncrypt.Write(lengthBytes, 0, lengthBytes.Length);
      msEncrypt.Write(encryptedBlock, 0, encryptedBlock.Length);

      offset += blockSize;
    }

    return Convert.ToBase64String(msEncrypt.ToArray());
  }

  /// <summary>
  /// 使用私鑰解密資料
  /// </summary>
  public string Decrypt(string cipherText)
  {
    if (string.IsNullOrWhiteSpace(cipherText))
    {
      return cipherText;
    }

    try
    {
      var cipherBytes = Convert.FromBase64String(cipherText);
      var keySize = _rsa.KeySize;
      var maxBlockSize = (keySize / 8) - 42;

      // 檢查是否為分段加密
      if (cipherBytes.Length <= (keySize / 8))
      {
        // 單段加密
        var decryptedBytes = _rsa.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedBytes);
      }

      // 分段解密
      using var msDecrypt = new MemoryStream();
      var offset = 0;

      while (offset < cipherBytes.Length)
      {
        var lengthBytes = new byte[4];
        Array.Copy(cipherBytes, offset, lengthBytes, 0, 4);
        var blockLength = BitConverter.ToInt32(lengthBytes, 0);
        offset += 4;

        var encryptedBlock = new byte[blockLength];
        Array.Copy(cipherBytes, offset, encryptedBlock, 0, blockLength);
        offset += blockLength;

        var decryptedBlock = _rsa.Decrypt(encryptedBlock, RSAEncryptionPadding.OaepSHA256);
        msDecrypt.Write(decryptedBlock, 0, decryptedBlock.Length);
      }

      return Encoding.UTF8.GetString(msDecrypt.ToArray());
    }
    catch (FormatException)
    {
      throw new CryptographicException("Invalid cipher text format. Expected Base64 encoded string.");
    }
    catch (CryptographicException ex)
    {
      throw new CryptographicException("Decryption failed. Please check the RSA private key.", ex);
    }
  }

  /// <summary>
  /// 使用私鑰簽章資料
  /// </summary>
  public string SignData(string data)
  {
    if (string.IsNullOrWhiteSpace(data))
    {
      throw new ArgumentException("Data cannot be null or empty for signing.", nameof(data));
    }

    var dataBytes = Encoding.UTF8.GetBytes(data);
    var signatureBytes = _rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    return Convert.ToBase64String(signatureBytes);
  }

  /// <summary>
  /// 使用公鑰驗證簽章
  /// </summary>
  public bool VerifySignature(string data, string signature)
  {
    if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(signature))
    {
      return false;
    }

    try
    {
      var dataBytes = Encoding.UTF8.GetBytes(data);
      var signatureBytes = Convert.FromBase64String(signature);
      return _rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
    catch
    {
      return false;
    }
  }

  /// <summary>
  /// 取得公鑰（Base64 編碼）
  /// </summary>
  public string GetPublicKey()
  {
    var publicKeyBytes = _rsa.ExportRSAPublicKey();
    return Convert.ToBase64String(publicKeyBytes);
  }

  /// <summary>
  /// 取得私鑰（Base64 編碼，僅用於內部解密和簽章）
  /// </summary>
  public string GetPrivateKey()
  {
    var privateKeyBytes = _rsa.ExportRSAPrivateKey();
    return Convert.ToBase64String(privateKeyBytes);
  }

  public void Dispose()
  {
    if (_disposeRsa)
    {
      _rsa?.Dispose();
    }
  }
}
