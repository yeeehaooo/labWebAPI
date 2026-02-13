using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DMIS_Backend.Infrastructure.Security.Cryptography.Symmetric;

/// <summary>
/// AES256 加密/解密提供者實作
/// </summary>
public class Aes256EncryptionProvider : IEncryptionProvider
{
  private readonly byte[] _key;
  private const int TagSize = 16;
  private const int NonceSize = 12;

  public Aes256EncryptionProvider(IConfiguration config)
  {
    _key = Convert.FromBase64String(config["Encryption:Key"] ?? "");
    if (_key.Length != 32)
    {
      throw new InvalidOperationException("AES256 key required.");
    }
  }

  public string Encrypt(string plainText)
  {
    if (string.IsNullOrEmpty(plainText))
    {
      return plainText;
    }

    byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
    byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
    byte[] cipher = new byte[plaintextBytes.Length];
    byte[] tag = new byte[TagSize];

    using var aes = new AesGcm(_key, TagSize);
    aes.Encrypt(nonce, plaintextBytes, cipher, tag);

    return Convert.ToBase64String(nonce.Concat(tag).Concat(cipher).ToArray());
  }

  public string Decrypt(string cipherText)
  {
    byte[] data = Convert.FromBase64String(cipherText);

    byte[] nonce = data[..NonceSize];
    byte[] tag = data[NonceSize..(NonceSize + TagSize)];
    byte[] cipher = data[(NonceSize + TagSize)..];

    byte[] plaintext = new byte[cipher.Length];

    using var aes = new AesGcm(_key, TagSize);
    aes.Decrypt(nonce, cipher, tag, plaintext);

    return Encoding.UTF8.GetString(plaintext);
  }
}
