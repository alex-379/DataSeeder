using System.Security.Cryptography;
using System.Text;

namespace DataSeeder;

public static class PasswordsService
{
    private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;
    private const int keySize = 64;
    private const int iterations = 350000;

    public static (string hash, string salt) HashPassword(string password, string secret)
    {
        password = $"{password}{secret}";
        var salt = RandomNumberGenerator.GetBytes(keySize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            iterations,
            _hashAlgorithm,
            keySize);

        return (Convert.ToHexString(hash), Convert.ToHexString(salt));
    }
}
