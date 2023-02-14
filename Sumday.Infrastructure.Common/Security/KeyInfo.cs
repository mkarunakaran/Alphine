using System;
using System.Security.Cryptography;

namespace Sumday.Infrastructure.Common.Security
{
    public class KeyInfo
    {
        public KeyInfo()
        {
            using var myAes = Aes.Create();
            this.Key = myAes.Key;
            this.Iv = myAes.IV;
        }

        public KeyInfo(string key, string iv)
        {
            this.Key = Convert.FromBase64String(key);
            this.Iv = Convert.FromBase64String(iv);
        }

        public KeyInfo(byte[] key, byte[] iv)
        {
            this.Key = key;
            this.Iv = iv;
        }

        public byte[] Key { get; }

        public byte[] Iv { get; }

        public string KeyString => Convert.ToBase64String(this.Key);

        public string IVString => Convert.ToBase64String(this.Iv);

        public bool HasValues()
        {
            return !string.IsNullOrWhiteSpace(this.KeyString) && !string.IsNullOrWhiteSpace(this.IVString);
        }
    }
}
