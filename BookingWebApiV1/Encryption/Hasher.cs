using System.Security.Cryptography;
using BookingWebApiV1.Models.DatabaseDTOs;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BookingWebApiV1.Encryption;

public static class Hasher
{
        // Hardcoded Pepper, because its used on everyone's passwords.
        private const string Pepper = "xndrAvQhQJbQpwzF";
        
        /// <summary>
        /// This method generate a random salt (32 byte array)  
        /// </summary>
        /// <returns> Base64string of an randomized 32 byte array </returns>
        public static string GenerateSalt()
        {
            // 256 bit salt
            var byteArray = new byte[32];

            // fill 32 byte array with random values
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(byteArray);
            }

            return Convert.ToBase64String(byteArray);
        }


        /// <summary>
        /// This method returns the base64string of 128bit salt and the hashed password
        /// </summary>
        /// <param name="password">Orginal password</param>
        /// <param name="salt">Base64String of a randomized byte array</param>
        /// <returns> The a hash of password and salt </returns>
        public static string SaltAndHashPassword(string password, string salt)
        {
            // hash the password with the salt
            string hashedPassword = HashPassword(salt, password + Pepper);

            return hashedPassword;
        }


        /// <summary>
        /// hash the password with the salt xamount of times
        /// </summary>
        /// <param name="salt"> base64string of a randomized byte array</param>
        /// <param name="passwordToHash">original password</param>
        /// <returns> Base64String with hashed password </returns>
        private static string HashPassword(string salt, string passwordToHash)
        {
            var saltBytes = Convert.FromBase64String(salt);

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToHash,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }


        /// <summary>
        /// This method is used to validate the input password from the user with the hashed password stored in the database
        /// </summary>
        /// <param name="password">Original password</param>
        /// <param name="user">The user object from the database</param>
        /// <returns> True if password is a match </returns>
        public static bool ValidatePassword(string password, UserDTO user)
        {
            var userSalt = user.PasswordSalt;
            
            var hashedPassword = HashPassword(userSalt, password + Pepper);


            return hashedPassword == user.Password;
        }
}