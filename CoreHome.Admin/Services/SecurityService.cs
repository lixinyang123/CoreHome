using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace CoreHome.Admin.Services
{
    public class SecurityService
    {
        private readonly RijndaelManaged rijndaelManaged;
        private readonly string configPath;
        private readonly string configFile;

        private string Key
        {
            get
            {
                return File.ReadAllText(configFile);
            }
            set
            {
                File.WriteAllText(configFile, value);
            }
        }

        public SecurityService()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                configPath = @"C:/Server/CoreHome/";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                configPath = @"/home/Server/CoreHome/";
            }

            configFile = configPath + "key.txt";

            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            if (!File.Exists(configFile))
            {
                Key = Guid.NewGuid().ToString().Replace("-", "");
            }

            rijndaelManaged = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(Key),
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
