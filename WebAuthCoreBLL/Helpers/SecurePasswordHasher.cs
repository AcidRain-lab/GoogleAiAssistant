using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebAuthCoreBLL.Helpers
{
   
    public class SecurePasswordHasher
    {
        public static string Hash(string password)
        {
            // Генерируем соль
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Создаем хеш
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Возвращаем соль и хеш, объединенные в одной строке
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }


       
        public static bool Verify(string password, string storedHash)
        {
            // Разделяем строку на соль и хеш
            var parts = storedHash.Split(':', 2);
            if (parts.Length != 2)
            {
                throw new FormatException("Wrong hash format.");
            }

            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            // Создаем хеш из пароля с использованием соли
            var hashOfEnteredPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Сравниваем полученный хеш с сохраненным хешем
            return hashOfEnteredPassword == hash;
        }

    }
}
