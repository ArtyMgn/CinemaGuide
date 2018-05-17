using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CinemaGuide.Utils
{
    public class PasswordEncryptor
    {
        private const int HASH_ITERATIONS = 43;

        public static byte[] GenerateSalt(byte length = 64)
        {
            var rng  = new RNGCryptoServiceProvider();
            var salt = new byte[length];
            rng.GetBytes(salt);
            return salt;
        }

        public static string GenerateHash(string clearTextData, byte[] salt)
        {
            if (!(salt?.Length > 0))
            {
                throw new ArgumentException(
                    $"Salt parameter {nameof(salt)} cannot be empty or null. " +
                    "This is a security violation.");
            }

            var hashedPassword = KeyDerivation.Pbkdf2(
                clearTextData,
                salt,
                KeyDerivationPrf.HMACSHA512,
                HASH_ITERATIONS, 32
            );
            return Convert.ToBase64String(hashedPassword);

        }

        public static bool IsEqualPasswords(string storedHashedPwd, string storedSalt, string clearTextPwd)
        {
            try
            {
                var userEnteredHashedPwd = Convert.FromBase64String(GenerateHash(clearTextPwd, Convert.FromBase64String(storedSalt)));
                var originalHashedPwd    = Convert.FromBase64String(storedHashedPwd);
                if (userEnteredHashedPwd.SequenceEqual(originalHashedPwd))
                {
                    return true;
                }
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
                return false;
            }

            return false;
        }
    }
}
