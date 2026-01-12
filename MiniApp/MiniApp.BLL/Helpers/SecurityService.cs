using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace MiniApp.BLL.Helpers
{
    public static class SecurityService
    {

        public static byte[] GenerateSalt() => RandomNumberGenerator.GetBytes(16);
        public static string GenerateVerificationCode()=> RandomNumberGenerator.GetInt32(100000, 999999).ToString();

        public static string PasswordHash(string password, byte[] salt)
        {
            byte[] hashByte= KeyDerivation.Pbkdf2(
             password,
             salt,
             KeyDerivationPrf.HMACSHA256,
             32,
             100000);
            return Convert.ToBase64String(hashByte);
        }
        public static bool VerifyPassword(string password, byte[] salt,string correctPassword)
        {
            if(correctPassword != PasswordHash(password,salt))
                return false;
            return true;
        }

    }
}
