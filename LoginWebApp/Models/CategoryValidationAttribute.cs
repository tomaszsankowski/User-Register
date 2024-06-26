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

            // Uzyskanie dost�pu do UserContext czyli naszej bazy danych
            UserContext? userContext = validationContext.GetService(typeof(UserContext)) as UserContext ?? throw new InvalidOperationException("Nie mo�na uzyska� dost�pu do bazy danych.");

            // Sprawdzenie, czy kategoria zawiera si� w s�owniku
            if (!userContext.Categories.Any(c => c.Name == category))
            {
                return new ValidationResult("Nieprawid�owa kategoria.");
            }

            // S�u�bowy: podkategoria musi zawiera� si� w s�owniku
            if (category == "S�u�bowy" && (!userContext.Subcategories.Any(sc => sc.Name == subcategory)))
            {
                return new ValidationResult("Dla kategorii 'S�u�bowy' wymagana jest podkategoria ze s�ownika.");
            }
            // Inny: podkategoria to dowolny string
            else if (category == "Inny" && (string.IsNullOrWhiteSpace(subcategory)))
            {
                return new ValidationResult("Dla kategorii 'Inny' wymagana jest dowolna podkategoria.");
            }
            // Prywatny: nie ma podkategorii
            else if (category == "Prywatny" && (subcategory != null))
            {
                return new ValidationResult("Dla kategorii 'Prywatny' podkategoria musi by� pusta.");
            }

            // Zwr�cenie sukcesu, je�li obiekt spe�nia postawione przez nas warunki
            return ValidationResult.Success;
        }
    }

    // Analogiczna klasa jak wy�ej, z tym �e z rzutowaniem na User (wiem, powielanie kodu, pewnie da�o si� to zrobi� lepiej...)
    public class CategoryValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Pobranie gracza, jego kategorii i podkategorii
            var user = (User)validationContext.ObjectInstance;
            var category = user.Category;
            var subcategory = user.Subcategory;

            // Uzyskanie dost�pu do UserContext czyli naszej bazy danych
            UserContext? userContext = validationContext.GetService(typeof(UserContext)) as UserContext ?? throw new InvalidOperationException("Nie mo�na uzyska� dost�pu do bazy danych.");

            // Sprawdzenie, czy kategoria zawiera si� w s�owniku
            if (!userContext.Categories.Any(c => c.Name == category))
            {
                return new ValidationResult("Nieprawid�owa kategoria.");
            }

            // S�u�bowy: podkategoria musi zawiera� si� w s�owniku
            if (category == "S�u�bowy" && (!userContext.Subcategories.Any(sc => sc.Name == subcategory)))
            {
                return new ValidationResult("Dla kategorii 'S�u�bowy' wymagana jest podkategoria ze s�ownika.");
            }
            // Inny: podkategoria to dowolny string
            else if (category == "Inny" && (string.IsNullOrWhiteSpace(subcategory)))
            {
                return new ValidationResult("Dla kategorii 'Inny' wymagana jest dowolna podkategoria.");
            }
            // Prywatny: nie ma podkategorii
            else if (category == "Prywatny" && (subcategory != null))
            {
                return new ValidationResult("Dla kategorii 'Prywatny' podkategoria musi by� pusta.");
            }

            // Zwr�cenie sukcesu, je�li obiekt spe�nia postawione przez nas warunki
            return ValidationResult.Success;
        }
    }
}