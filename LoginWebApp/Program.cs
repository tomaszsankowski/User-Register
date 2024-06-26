using LoginWebApp.Data;
using LoginWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using LoginWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dodanie kontekstu bazy danych
builder.Services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("OurUser"));

// Dodanie SecurityServices
builder.Services.AddScoped<SecurityServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pozwolenie u¿ytkownikowi localhost pod portem 4200 na wysy³anie zapytañ do bazy danych
app.UseCors(options =>
options.WithOrigins("http://localhost:4200")
.AllowAnyMethod()
.AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

// Tworzenie konta administratora oraz s³owników kategorii i podkategorii
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UserContext>();
    context.Database.EnsureCreated();

    // Inicjalizacja danych s³ownikowych (kategorii i podkategorii)
    if (!context.Categories.Any())
    {
        var categories = new List<Category>
        {
            new Category { Name = "S³u¿bowy" },
            new Category { Name = "Prywatny" },
            new Category { Name = "Inny" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        var subcategories = new List<Subcategory>
        {
            new Subcategory { Name = "Intern" },
            new Subcategory { Name = "Junior" },
            new Subcategory { Name = "Mid" },
            new Subcategory { Name = "Senior" },
            new Subcategory { Name = "Manager" }
        };

        context.Subcategories.AddRange(subcategories);
        context.SaveChanges();
    }

    // Tworzenie konta administratora
    if (!context.Users.Any(u => u.Name == "admin"))
    {
        var adminUser = new User
        {
            Name = "admin",
            Password = SecurityServices.HashPassword("Admin123!"), // Zahashowane has³o administratora bêdzie przechowywane na serwerze
            Surname = "admin",
            Email = "admin@admin",
            Phone = "000000000",
            Category ="S³u¿bowy",
            Subcategory = "Senior",
            DateOfBirth = new DateOnly(1900, 1, 1)
        };
        context.Users.Add(adminUser);
        context.SaveChanges();
    }
}

app.Run();
