using CoreHome.Admin.Models;
using CoreHome.Infrastructure.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CoreHome.Admin.Services
{
    public class SecurityService : StaticConfig<Secret>
    {
        private readonly RijndaelManaged rijndaelManaged;

        public SecurityService(string fileName, Secret initSecret) : base(fileName, initSecret)
        {
            rijndaelManaged = new RijndaelManaged
            {
                // 初始化向量
                IV = Encoding.UTF8.GetBytes(Config.IV),
                // 密钥
                Key = Encoding.UTF8.GetBytes(Config.Key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
        }

        public string AESEncrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            using ICryptoTransform cTransform = rijndaelManaged.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string AESDecrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Convert.FromBase64String(str);

            using ICryptoTransform cTransform = rijndaelManaged.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        public string SHA256Encrypt(string str)
        {
            byte[] password = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder builder = new();
            for (int i = 0; i < password.Length; i++)
            {
                builder.Append(password[i].ToString("X2"));
            }
            return builder.ToString();
        }

    }
}
