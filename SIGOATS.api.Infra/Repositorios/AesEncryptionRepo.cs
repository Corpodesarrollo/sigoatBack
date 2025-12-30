using Microsoft.Extensions.Options;
using SIGOATS.api.Core.DTO;
using System.Security.Cryptography;
using System.Text;

namespace SIGOATS.api.Infra.Repositorios
{
    public class AesEncryptionRepo(IOptions<EncryptionSettingsDto> encryptionSettings)
    {
        private readonly EncryptionSettingsDto _encryptionSettings = encryptionSettings.Value;

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.KeySize = 256;
                aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionSettings.Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(_encryptionSettings.IV);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new())
                {
                    using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_encryptionSettings.Key);
            aes.IV = Encoding.UTF8.GetBytes(_encryptionSettings.IV);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream msDecrypt = new(Convert.FromBase64String(cipherText));
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
