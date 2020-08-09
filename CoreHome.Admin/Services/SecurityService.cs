using CoreHome.Infrastructure.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CoreHome.Admin.Services
{
    public class SecurityService : StaticConfig<string>
    {
        private readonly RijndaelManaged rijndaelManaged;

        public SecurityService(string fileName, string initKey) : base(fileName, initKey)
        {
            rijndaelManaged = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(Config),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
        }

        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            ICryptoTransform cTransform = rijndaelManaged.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            byte[] toEncryptArray = Convert.FromBase64String(str);

            ICryptoTransform cTransform = rijndaelManaged.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

    }
}
