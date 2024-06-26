using System.ComponentModel.DataAnnotations;

namespace LoginWebApp.Models
{
    // Klasa DTO na podstawie klasy User, której obiekty nie zawierają hasła (a dokładniej hashu hasła). Oczywiście uniemożliwia to zmianę hasła, ale pominąłem tą funkcjonalność
    public class UserDTO
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

        [Required]
        [StringLength(9)]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi składać się z 9 cyfr.")]
        public required string Phone { get; set; }

        [Required]
        [CategoryValidationDTO] // Walidacja kategorii i podkategorii
        public required string Category { get; set; }

        public string? Subcategory { get; set; } // Tylko dla kategorii Inny i Służbowy

        [Required]
        public required DateOnly DateOfBirth { get; set; }
    }
}
