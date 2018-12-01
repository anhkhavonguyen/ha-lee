using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Harvey.Ids.Utils
{
    public static class StringExtension
    {
        public static string RandomString()
        {
            byte[] bytes = BitConverter.GetBytes(Farmhash.Sharp.Farmhash.Hash32(Guid.NewGuid().ToString()));
            return string.Join("", bytes.Select(s => s.ToString("X")));
        }

        public static string ToMd5String(this string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GeneratePIN()
        {
            string chars = "1234567890";
            char[] stringChars = new char[4];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            return finalString;
        }
    }
}
