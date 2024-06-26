using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Klasa użytkownik
namespace LoginWebApp.Models
{
    // Klasa użytkownika, posiadająca wszystkie pola przewidziane w zadaniu. Już tutaj zapewniona jest walidacja niektórych pól
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(100)]
        public required string Surname { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        // Hasło: przynajmniej jedna mała litera, jedna duża litera, jedna cyfra, jeden znak specjalny i mieć przynajmniej 6 znaków długości
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$", ErrorMessage = "Hasło musi zawierać przynajmniej jedną dużą literę, jedną małą literę, jedną cyfrę, jeden znak specjalny i mieć przynajmniej 6 znaków długości.")]
        public required string Password { get; set; } // Przechowywany będzie hash hasła

        [Required]
        [StringLength(9)]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi składać się z 9 cyfr.")]
        public required string Phone { get; set; }

        [Required]
        [CategoryValidation] // Walidacja kategorii i podkategorii
        public required string Category { get; set; }

        public string? Subcategory { get; set; } // Tylko dla kategorii Inny i Służbowy

        [Required]
        public required DateOnly DateOfBirth { get; set; }
    }

    // Klasa kategoria (tylko dla bazy danych)
    public class Category
    {
        [Key]
        [Required]
        public required string Name { get; set; }
    }

    // Klasa podkategoria (tylko dla bazy danych)
    public class Subcategory
    {
        [Key]
        [Required]
        public required string Name { get; set; }
    }
}