using LoginWebApp.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// Walidacja kategorii i podkategorii. Osobna klasa dla User oraz UserDTO
namespace LoginWebApp.Models
{
    public class CategoryValidationDTOAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Pobranie gracza, jego kategorii i podkategorii
            var user = (UserDTO)validationContext.ObjectInstance;
            var category = user.Category;
            var subcategory = user.Subcategory;

            // Uzyskanie dostêpu do UserContext czyli naszej bazy danych
            UserContext? userContext = validationContext.GetService(typeof(UserContext)) as UserContext ?? throw new InvalidOperationException("Nie mo¿na uzyskaæ dostêpu do bazy danych.");

            // Sprawdzenie, czy kategoria zawiera siê w s³owniku
            if (!userContext.Categories.Any(c => c.Name == category))
            {
                return new ValidationResult("Nieprawid³owa kategoria.");
            }

            // S³u¿bowy: podkategoria musi zawieraæ siê w s³owniku
            if (category == "S³u¿bowy" && (!userContext.Subcategories.Any(sc => sc.Name == subcategory)))
            {
                return new ValidationResult("Dla kategorii 'S³u¿bowy' wymagana jest podkategoria ze s³ownika.");
            }
            // Inny: podkategoria to dowolny string
            else if (category == "Inny" && (string.IsNullOrWhiteSpace(subcategory)))
            {
                return new ValidationResult("Dla kategorii 'Inny' wymagana jest dowolna podkategoria.");
            }
            // Prywatny: nie ma podkategorii
            else if (category == "Prywatny" && (subcategory != null))
            {
                return new ValidationResult("Dla kategorii 'Prywatny' podkategoria musi byæ pusta.");
            }

            // Zwrócenie sukcesu, jeœli obiekt spe³nia postawione przez nas warunki
            return ValidationResult.Success;
        }
    }

    // Analogiczna klasa jak wy¿ej, z tym ¿e z rzutowaniem na User (wiem, powielanie kodu, pewnie da³o siê to zrobiæ lepiej...)
    public class CategoryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Pobranie gracza, jego kategorii i podkategorii
            var user = (User)validationContext.ObjectInstance;
            var category = user.Category;
            var subcategory = user.Subcategory;

            // Uzyskanie dostêpu do UserContext czyli naszej bazy danych
            UserContext? userContext = validationContext.GetService(typeof(UserContext)) as UserContext ?? throw new InvalidOperationException("Nie mo¿na uzyskaæ dostêpu do bazy danych.");

            // Sprawdzenie, czy kategoria zawiera siê w s³owniku
            if (!userContext.Categories.Any(c => c.Name == category))
            {
                return new ValidationResult("Nieprawid³owa kategoria.");
            }

            // S³u¿bowy: podkategoria musi zawieraæ siê w s³owniku
            if (category == "S³u¿bowy" && (!userContext.Subcategories.Any(sc => sc.Name == subcategory)))
            {
                return new ValidationResult("Dla kategorii 'S³u¿bowy' wymagana jest podkategoria ze s³ownika.");
            }
            // Inny: podkategoria to dowolny string
            else if (category == "Inny" && (string.IsNullOrWhiteSpace(subcategory)))
            {
                return new ValidationResult("Dla kategorii 'Inny' wymagana jest dowolna podkategoria.");
            }
            // Prywatny: nie ma podkategorii
            else if (category == "Prywatny" && (subcategory != null))
            {
                return new ValidationResult("Dla kategorii 'Prywatny' podkategoria musi byæ pusta.");
            }

            // Zwrócenie sukcesu, jeœli obiekt spe³nia postawione przez nas warunki
            return ValidationResult.Success;
        }
    }
}