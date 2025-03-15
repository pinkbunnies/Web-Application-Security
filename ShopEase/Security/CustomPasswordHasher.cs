using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ShopApp.Models;

namespace ShopApp.Security
{
    public class CustomPasswordHasher : IPasswordHasher<ApplicationUser>
    {
        private const string Salt = "Dificil_";
        public string HashPassword(ApplicationUser user, string password)
        {
            using (var md5 = MD5.Create())
            {
                var combined = Salt + password;
                var bytes = Encoding.UTF8.GetBytes(combined);
                var hash = md5.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            var providedHash = HashPassword(user, providedPassword);
            return providedHash == hashedPassword ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
