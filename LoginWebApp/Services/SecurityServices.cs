using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using LoginWebApp.Models;
using LoginWebApp.Data;

namespace LoginWebApp.Services
{
    // Klasa odpowiedzialna za hashowanie haseł, sprawdzanie hashy przy logowaniu oraz ogarnianie SessionTokenów
    public class SecurityServices(UserContext context)
    {
        private readonly UserContext _context = context;

        // Tworzy hash hasła
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Porównuje hash hasła w bazie z podanym przez użytkownika hasłem
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // Tworzy nowy unikalny SessionToken i dodaje go do bazy
        public async Task<string> GenerateSessionTokenAsync(string usermail)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            while (await _context.SessionTokens.AnyAsync(st => st.Token == token))
            {
                token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            }

            var sessionToken = new SessionToken
            {
                Token = token,
                UserId = usermail,
            };

            _context.SessionTokens.Add(sessionToken);
            await _context.SaveChangesAsync();

            return token;
        }

        // Sprawdza, czy podany przez użytkownika token istnieje w bazie
        public async Task<bool> ValidateSessionTokenAsync(string token)
        {
            var sessionToken = await _context.SessionTokens
                .FirstOrDefaultAsync(st => st.Token == token);

            return sessionToken != null;
        }
    }
}
