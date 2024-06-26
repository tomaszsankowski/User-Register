# User-Register
User Register web app. Backend in C# using REST API, frontend in Angular. InMemory database.
-
Folder LoginWebApp zawiera cały backend, natomiast LoginWebAppClient cały frontend
-
Pliki, które powinny Państwa interesować:
-
Backend:
-Program.cs - plik inicjalizujący stronę serwera
-Services/SecurityServices.cs - funkcje obsługujące hashowanie haseł oraz obsługę własnostworzonego sessionToken
-Models/AuthenticadeModel.cs, Model/User.cs, Model/UserDTO.cs - klasy potrzebne do działania aplikacji
-Models/CategoryValidationAttribute - obsługa walidacji pól, zawierających dane słownikowe
-Data/UserContext.cs - klasa pośrednicząca w łączeniu się z bazą danych (baza danych InMemory)
-Controllers/UsersController.cs - klasa zawierająca wszystkie funkcje obsługujące żądania
-
Frontend:
-
-

[PL] Jak odpalić program?
-otworzyć plik .sln w folderze LoginWebApp, najlepiej w Visual Studio
-odpalić program backendu (powinno otworzyć przeglądarkę ze stroną zapoznawczą Swaggera
-otworzyć folder z frontendem, najlepiej w VSCode
-otworzyć terminal w VSCode i wpisać komendę 'ng serve -o'
-powinno otworzyć kolejną kartę przeglądarki, w której otworzy się już nasza aplikacja
-baza danych jest typu InMemory, dlatego zapisane rekordy usuną się po zakończeniu się procesu backendu
-dane słownikowe są automatycznie dodawane do bazy danych
-domyślnie istnieje tylko jeden użytkownik (administrator) który posiada następujące dane do logowania:
{ email: admin@admin, password: Admin123! }
-
