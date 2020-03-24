using System;
using System.Security.Cryptography;
using System.Text;

namespace CoreHome.Admin.Services
{
    public class SecurityService
    {
        private readonly Aes aes;

        public SecurityService()
        {
            aes = Aes.Create();
            aes.Key = GetAesKey(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
        }

        ~SecurityService()
        {
            aes.Dispose();
        }

        private byte[] GetAesKey(byte[] keyArray)
        {
            byte[] newArray = new byte[16];
            if (keyArray.Length < 16)
            {
                for (int i = 0; i < newArray.Length; i++)
                {
                    if (i >= keyArray.Length)
                    {
                        newArray[i] = 0;
                    }
                    else
                    {
                        newArray[i] = keyArray[i];
                    }
                }
            }
            return newArray;
        }

        public string Encryptor(string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            byte[] result = aes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
            return Convert.ToBase64String(result);
        }

        public string Decryptor(string content)
        {
            byte[] buffer = Convert.FromBase64String(content);
            byte[] result = aes.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(result);
        }

    }
}
