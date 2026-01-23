using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace Identity.BLL.Helpers
{
    public static class SecurityService
    {

        public static byte[] GenerateRandomNumber(int byteLength=64) => RandomNumberGenerator.GetBytes(16);
        public static string GenerateVerificationCode()=> RandomNumberGenerator.GetInt32(100000, 999999).ToString();

        public static string PasswordHash(string password, byte[] salt)
        {
            byte[] hashByte = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 32
            );
            return Convert.ToBase64String(hashByte);
        }
        public static string HashRefreshToken(string refreshToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
            return Convert.ToHexString(bytes);
        }
        public static bool VerifyRefreshToken(string incomingToken, string storedHashHex)
        {
            var incomingHash = HashRefreshToken(incomingToken);
            var newHashed = Convert.FromHexString(incomingHash);
            var oldhashed = Convert.FromHexString(storedHashHex);
            return CryptographicOperations.FixedTimeEquals(newHashed, oldhashed);
        }
        public static bool VerifyPassword(string password, byte[] salt,string correctPassword)
        {
            if(correctPassword != PasswordHash(password,salt))
                return false;
            return true;
        }

    }
}
