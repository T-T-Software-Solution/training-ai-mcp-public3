using System.Security.Cryptography;
using System.Text;

namespace App.AppCore.Applications
{
    public static class HashUtility
    {
        public static string ComputeSha256(string input)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + "1234"));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}