using System.Security.Cryptography;

namespace RFAuth.Util
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
            var token = Convert.ToBase64String(randomBytes);
            return token[..size];
        }
    }
}
