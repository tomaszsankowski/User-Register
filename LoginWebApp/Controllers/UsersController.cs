using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginWebApp.Data;
using LoginWebApp.Models;
using LoginWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NuGet.Common;

namespace LoginWebApp.Controllers
{
    // Klasa zawierająca wszystkie funkcje obsługujące żądania dotyczące użytkowników
    // Zawiera funkcje pobierające wszystkich użytkowników, wybranego po Id użytkownika, modyfikację istniejącego użytkownika,
    // Dodawanie nowych użytkowników oraz usuwanie użytkowników. Ponadto zawiera funkcję odpowiedzialną za logowanie i wylogowywanie.
    // Funkcje zmieniające, dodające i usuwające użytkowników wymagają podania w nagłówku żądania swojego SessionKey
    // Następnie sprawdzają, czy dany SessionKey istnieje w bazie, jeśli nie to nie obsługują żądania.
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserContext context, SecurityServices securityServices) : ControllerBase
    {
        // Kontekst bazy danych
        private readonly UserContext _context = context;

        // Obiekt SecurityService do obsługi SessionTokenów
        private readonly SecurityServices _securityServices = securityServices;


        // Metoda zwracająca listę wszystkich użytkowników
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            // Zwraca użytkowników jako klasy DTO, niezawierające hasła (a dokładniej hashu hasła z bazy)
            return await _context.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Phone = user.Phone,
                    DateOfBirth = user.DateOfBirth,
                    Category = user.Category,
                    Subcategory = user.Subcategory,
                })
                .ToListAsync();
        }

        // Metoda zwracająca konkretnego użytkownika
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            // Znajdywanie użytkownka
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Zwracanie klasy DTO, żeby nie zawiarała ona hasła (a dokłądniej hashu hasła)
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
                Category = user.Category,
                Subcategory = user.Subcategory,
            };
        }

        // Metoda modyfikująca wybranego użykownika
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO user)
        {
            // Pobranie tokenu z nagłówka żądania
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Brak tokenu.");
            }

            var token = authorizationHeader["Bearer ".Length..].Trim();

            // Sprawdzenie, czy token istnieje w bazie danych
            var tokenExists = await _context.SessionTokens.AnyAsync(t => t.Token == token);
            if (!tokenExists)
            {
                return Unauthorized("Token nie istnieje lub jest nieprawidłowy.");
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            // Sprawdzenie, czy model przeszedł walidację i wysłanie opisu błędów jeśli nie przeszedł walidacji
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value != null ? kvp.Value.Errors
                                    .Where(error => error.ErrorMessage != null)
                                    .Select(e => e.ErrorMessage).ToArray()
                                  : Array.Empty<string>()
                    );

                return BadRequest( new { errors });
            }

            // Dodatkowe sprawdzenie unikalności emaila
            var existingUser = await _context.Users
                .Where(u => u.Id != id) // Pomiń bieżącego użytkownika
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { error = "Konto z podanym adresem email już istnieje!" });
            }

            // Pobranie szukanego użytkowika z bazy danych w celu przywrócenia hasła
            var currentUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (currentUser == null)
            {
                return NotFound();
            }

            var currentPasswordHash = currentUser.Password;

            // Do bazy przekazujemy zaktualizowanego uzytkownika, ale hasło zostawiamy to samo
            _context.Entry(new User
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Password = currentPasswordHash,
                Email = user.Email,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
                Category = user.Category,
                Subcategory = user.Subcategory,
            }).State = EntityState.Modified;

            // Na koniec zatwierdzamy zmiany
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Zwrócenie aktualnej listy użytkowników (klasy DTO!)
            return Ok(await _context.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Phone = user.Phone,
                    DateOfBirth = user.DateOfBirth,
                    Category = user.Category,
                    Subcategory = user.Subcategory,
                })
                .ToListAsync());
        }

        // Metoda wysyłająca na serwer nowego użytkownika
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Pobranie tokena z nagłówka żądania
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Brak tokenu.");
            }

            var token = authorizationHeader["Bearer ".Length..].Trim();

            // Sprawdzenie, czy token istnieje w bazie danych
            var tokenExists = await _context.SessionTokens.AnyAsync(t => t.Token == token);
            if (!tokenExists)
            {
                return Unauthorized("Token nie istnieje lub jest nieprawidłowy.");
            }

            // Sprawdzenie, czy model przeszedł walidację i wysłanie opisu błędów jeśli nie przeszedł walidacji
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value != null ? kvp.Value.Errors
                                    .Where(error => error.ErrorMessage != null)
                                    .Select(e => e.ErrorMessage).ToArray()
                                  : Array.Empty<string>()
                    );

                return BadRequest(new { errors });
            }

            // Dodatkowe sprawdzenie unikalności emaila
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                // Zwrócenie błędu, jeśli email już istnieje w bazie danych
                return BadRequest(new { error = "Konto z podanym adresem email już istnieje!" });
            }

            // Zahashowanie hasła i dodanie użytkownika
            var hashedPassword = SecurityServices.HashPassword(user.Password);
            user.Password = hashedPassword;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Zwrócenie aktualnej listy użytkowników (klasy DTO!)
            return Ok(await _context.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Phone = user.Phone,
                    DateOfBirth = user.DateOfBirth,
                    Category = user.Category,
                    Subcategory = user.Subcategory,
                })
                .ToListAsync());
        }

        // Metoda usuwająca wybranego użytkownika
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Pobranie tokena z nagłówka żądania
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Brak tokenu.");
            }

            var token = authorizationHeader["Bearer ".Length..].Trim();

            // Sprawdzenie, czy token istnieje w bazie danych
            var tokenExists = await _context.SessionTokens.AnyAsync(t => t.Token == token);
            if (!tokenExists)
            {
                return Unauthorized("Token nie istnieje lub jest nieprawidłowy.");
            }

            // Znalezienia użytkownika, którego chcemy usunąć
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Usunięcie użytkownika i zwrócenie aktualnej listy użytkowników
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // Zwrócenie aktualnej listy użytkowników (klasy DTO!)
            return Ok(await _context.Users
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Phone = user.Phone,
                    DateOfBirth = user.DateOfBirth,
                    Category = user.Category,
                    Subcategory = user.Subcategory,
                })
                .ToListAsync());
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // Metoda do logowania się
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateModel model)
        {
            // Sprawdzenie, czy użytkownik o podanym emailu istnieje w bazie danych
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user != null && SecurityServices.VerifyPassword(model.Password, user.Password))
            {
                // Użytkownik istnieje i hasło jest poprawne, logowanie powiodło się
                // Generowanie tokena i zwrócenie go w odpowiedzi
                var token = await _securityServices.GenerateSessionTokenAsync(user.Email);
                return Ok(new { Success = true, Token = token });
            }
            else
            {
                // Użytkownik nie istnieje lub hasło jest niepoprawne, logowanie nie powiodło się
                return Ok(new { Success = false });
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Odbieranie tokenu z nagłówka Authorization
            var authorizationHeader = Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Brak tokenu.");
            }

            var token = authorizationHeader["Bearer ".Length..].Trim();

            // Wyszukiwanie i usuwanie tokenu z bazy danych
            var tokenEntity = await _context.SessionTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (tokenEntity != null)
            {
                _context.SessionTokens.Remove(tokenEntity);
                await _context.SaveChangesAsync();
                return Ok("Wylogowano pomyślnie.");
            }

            return NotFound("Token nie został znaleziony.");
        }


    }
}
