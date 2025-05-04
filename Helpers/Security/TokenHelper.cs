using System;
using System.Security.Cryptography;

namespace Helpers.Security
{
    public static class TokenHelper
    {
        public static string GenerateToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static DateTime GetTokenExpiry()
        {
            // Set token to expire in 1 day
            return DateTime.UtcNow.AddDays(1);
        }
    }
}