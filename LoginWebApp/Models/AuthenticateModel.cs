using System.ComponentModel.DataAnnotations;

namespace LoginWebApp.Models
{
    // Klasa, której obiekty są używane do logowania. Zawiera tylko email (ponieważ musi być unikatowy, więc najłatwiej rozpoznawać użytkownika po nim)
    // oraz hasło, które podczas logowania zostanie zahashowane i porównane z hashem w bazie
    public class AuthenticateModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    // Klasa SessionToken służy przechowywaniu aktywnych tokenów w bazie. Ponadto pole UserId pozwala określić, który użytkownik wysłał dane żądanie
    public class SessionToken
    {
        [Key]
        public int Id { get; set; }
        public required string Token { get; set; }
        public required string UserId { get; set; }
    }
}
