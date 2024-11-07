using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace RFHttpAction.Util
{
    public class Token
    {
        static public byte[] GetBytes(int size)
        {
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            var randomBytes = new byte[size];
            randomNumberGenerator.GetBytes(randomBytes);

            return randomBytes;
        }

        static public string GetString(int size)
        {
            var randomBytes = GetBytes(size * 4 / 3 + 2);
            var token = BytesToBase62(randomBytes);
            return token[..size];
        }

        public static string BytesToBase62(byte[] bytes)
        {
            const string Base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            var base62 = new StringBuilder();
            var value = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);

            while (value > 0)
            {
                value = BigInteger.DivRem(value, 62, out var remainder);
                base62.Insert(0, Base62Chars[(int)remainder]);
            }

            return base62.ToString();
        }
    }
}
