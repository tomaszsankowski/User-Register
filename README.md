# User-Register
User Register web app. Backend in C# using REST API, frontend in Angular. InMemory database.
-

Folder LoginWebApp zawiera cały backend, natomiast LoginWebAppClient cały frontend
-

Pliki, które powinny Państwa interesować:
-

Backend (wykorzystane biblioteki: Microsoft.EntityFrameworkCore, System.Security.Cryptography, System.ComponentModel.DataAnnotations, Microsoft.AspNetCore.Mvc, Microsoft.AspNetCore.Authorization):
-

-Program.cs - plik inicjalizujący stronę serwera

-Services/SecurityServices.cs - funkcje obsługujące hashowanie haseł oraz obsługę własnostworzonego sessionToken

-Models/AuthenticadeModel.cs, Model/User.cs, Model/UserDTO.cs - klasy potrzebne do działania aplikacji

-Models/CategoryValidationAttribute - obsługa walidacji pól, zawierających dane słownikowe

-Data/UserContext.cs - klasa pośrednicząca w łączeniu się z bazą danych (baza danych InMemory)

-Controllers/UsersController.cs - klasa zawierająca wszystkie funkcje obsługujące żądania

Frontend (wykorzystane biblioteki: ngx-toastr, biblioteki wbudowane w Angulara):
-

-src/environments : zmienne środowiskowe (zawiera tylko adres strony serwera)

-src/app/app.config.ts : dodałem kilka providerów

-src/app/shared/auth.service.ts - odpowiedzialny ża ogarnianie logowania

-src/app/shared/user-details.model.ts - klasy niezbędne do obsługi żądań, klasy analogiczne jak w backendzie

-src/app/shared/user-details.service.ts - odpowidzialny za ogarnianie żądań dotyczących użytkowników

-src/app/shared/user-login.model.ts - klasa odpowiedzialna za przesyłanie danych do logowania, analogiczny odpowiednik na backendzie

-src/app/user-details/user-details.component.html - główny html zawierający nagłówek strony, wyświetlający odpowiedni formularz oraz listę użytkowników

-src/app/user-details/user-details.component.ts - obsługa naciśnięć listy użytkowników (usunięcie i modyfikacja istniejącego użytkownika)

-src/app/user-details/login-form/login-form.component.html - html zawierający formularz do logowania

-src/app/user-details/login-form/login-form.component.ts - obsługa przycisku logowania

-src/app/user-details/user-details-form/user-details-form.component.html - html zawierający formularz do dodawania/modyfikacji użytkowników

-src/app/user-details/user-details-form/user-details-form.component.ts - obsługa przycisku do dodawania/modyfikacji użytkowników

[PL] Jak odpalić program?
-

-otworzyć plik .sln w folderze LoginWebApp, najlepiej w Visual Studio

-odpalić program backendu (powinno otworzyć przeglądarkę ze stroną zapoznawczą Swaggera

-otworzyć folder z frontendem, najlepiej w VSCode

-otworzyć terminal w VSCode i wpisać komendę 'ng serve -o'

-powinno otworzyć kolejną kartę przeglądarki, w której otworzy się już nasza aplikacja

-baza danych jest typu InMemory, dlatego zapisane rekordy usuną się po zakończeniu się procesu backendu
-dane słownikowe są automatycznie dodawane do bazy danych

-domyślnie istnieje tylko jeden użytkownik (administrator) który posiada następujące dane do logowania:
{ email: admin@admin, password: Admin123! }

Program może wymagać instalacji pakietów NuGet w VisualStudio. Ponadto wymagana jest instalacja Node.js i Angular CLI. Baza danych InMemory nie wymaga dodatkowej konfiguracji, oprócz oczywiście odpowiednich pakietów NuGet.
-
