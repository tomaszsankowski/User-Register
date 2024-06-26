using Microsoft.EntityFrameworkCore;
using LoginWebApp.Models;

namespace LoginWebApp.Data
{
    // Klasa 'łącząca nas' z bazą danych, zawiera wszystkie tabele z bazy danych reprezentowane
    // za pomocą kolekcji DbSet oraz kolekcję aktywnych SessionTokenów
    public class UserContext(DbContextOptions options) : DbContext(options)
    {
        // Użytkownicy
        public DbSet<User> Users {  get; set; }

        // Słownik kategorii
        public DbSet<Category> Categories { get; set; }

        // Słownik podkategorii dla kategorii Służbowy
        public DbSet<Subcategory> Subcategories { get; set; }

        // SessionTokeny
        public DbSet<SessionToken> SessionTokens { get; set; }


        // Zapewnienie unikalności adresu email
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
