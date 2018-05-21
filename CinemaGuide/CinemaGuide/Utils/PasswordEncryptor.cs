using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CinemaGuide.Utils
{
    public class PasswordEncryptor
    {
        private const int hashIterations = 43;

        public static byte[] GenerateSalt(byte length = 64)
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[length];

            rng.GetBytes(salt);

            return salt;
        }

        public static string GenerateHash(string clearTextData, byte[] salt)
        {
            if (salt?.Length <= 0)
            {
                throw new ArgumentException(
                    $"Salt parameter {nameof(salt)} cannot be empty or null. " +
                    "This is a security violation.");
            }
            
            var hashedPassword = KeyDerivation.Pbkdf2(
                clearTextData,
                salt,
                KeyDerivationPrf.HMACSHA512,
                hashIterations, 32
            );

            return Convert.ToBase64String(hashedPassword);
        }

        public static bool IsEqualPasswords(string bHashedPassword, string bSalt, string password)
        {
            try
            {
                var hashedPassword = Convert.FromBase64String(bHashedPassword);
                var bHashedUserPassword = GenerateHash(password, Convert.FromBase64String(bSalt));
                var hashedUserPassword = Convert.FromBase64String(bHashedUserPassword);

                return hashedUserPassword.SequenceEqual(hashedPassword);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        public static string GenerateHash(string value)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
                return BitConverter.ToString(hash);
            }
        }
    }
}
