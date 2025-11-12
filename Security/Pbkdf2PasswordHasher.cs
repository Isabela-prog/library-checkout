using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;



namespace Library.Security
{
    // Implementação usando PBKDF2 (seguro e nativo do .NET)
    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        private const int Iterations = 100_000;
        private const int SaltSize = 16;
        private const int KeySize = 32;

        public string Hash(string password)
        {
            var salt = new byte[SaltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, KeySize);

            var outputBytes = new byte[1 + SaltSize + KeySize];
            outputBytes[0] = 0x01;
            Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, KeySize);
            return Convert.ToBase64String(outputBytes);
        }

        public bool Verify(string password, string hash)
        {
            var decoded = Convert.FromBase64String(hash);
            if (decoded.Length != 1 + SaltSize + KeySize || decoded[0] != 0x01) return false;
            var salt = new byte[SaltSize];
            Buffer.BlockCopy(decoded, 1, salt, 0, SaltSize);
            var storedSubkey = new byte[KeySize];
            Buffer.BlockCopy(decoded, 1 + SaltSize, storedSubkey, 0, KeySize);

            var generatedSubkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, KeySize);
            return CryptographicOperations.FixedTimeEquals(storedSubkey, generatedSubkey);
        }


    }
}
