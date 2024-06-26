import { Component } from '@angular/core';
import { UserDetailsService } from '../../shared/user-details.service';
import { FormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms';
import { UserDetails, UserDetailsDTO } from '../../shared/user-details.model';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-details-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './user-details-form.component.html',
  styles: ``
})
export class UserDetailsFormComponent {
  constructor(public service: UserDetailsService, private toastr:ToastrService) {

  }

  // Obsługa przycisku dodawania/modyfikacji nowego użytkownika
  onSubmit(form:NgForm) {

    // Usuwanie podkategorii, jeżeli wybrano kategorię 'Prywatny'
    if(this.service.formData.category == 'Prywatny')
    {
      this.service.formData.subcategory = null;
    }
    this.service.formSubmitted = true;

    // Sprawdzenie czy formularz jest poprawny
    if(form.valid == false)
    {
      return
    }

    // Wywołanie odpowiedniej funkcji w zależności od tego, czy dodajemy nowego użytkownika, czy modyfikujemy istniejącego
    if(this.service.formData.id == 0)
    {
      this.insertRecord(form)
    }
    else
    {
      this.updateRecord(form)
    }
  }

  // Funkcja dodająca nowego użytkownika
  insertRecord(form: NgForm) {
    this.service.postUser()
      .subscribe({
        next: res => {
          // Jeżeli dodanie powiodło się, serwer zwraca nam zaktualizowaną listę użytkowników, a my ją przekazujemy do serwisu
          this.service.list = (res as UserDetails[]).map(userDetails => ({
            userDetails: userDetails,
            detailsInfo: false
          }));
          this.service.resetForm(form);

          // Informacja o sukcesie
          this.toastr.success('Inserted successfully', 'User Register');
        },
        error: err => {
          if (err.status === 400 && err.error?.errors) {
            // Jeżeli serwer zwrócił błąd 400, to znaczy, że formularz nie przeszedł walidacji
            // Wyświetlamy więc użytkownikowi wszystkie błędy walidacji (zwracany jest słownik, który
            // pozwala określić, który input zawiera błąd i jaki jest ten błąd)
            Object.keys(err.error.errors).forEach(key => {
              err.error.errors[key].forEach((error: string) => {
                this.toastr.error(error, 'Validation Error');
              });
            });
          } else { // W przeciwnym wypadku wyświetlamy ogólny błąd
            this.toastr.error(err.error.error as string || 'An unknown error occurred', 'Error');
          }
        }
      })
  }
  
  // Funkcja aktualizująca istniejącego użytkownika
  updateRecord(form: NgForm) {
    this.service.isAddingNewUser = true;
    this.service.putUser()
      .subscribe({
        next: res => {
          // Jeżeli dodanie powiodło się, serwer zwraca nam zaktualizowaną listę użytkowników, a my ją przekazujemy do serwisu
          this.service.list = (res as UserDetailsDTO[]).map(userDetails => ({
            userDetails: userDetails,
            detailsInfo: false
          }));
          this.service.resetForm(form);

          // Informacja o sukcesie
          this.toastr.info('Updated successfully', 'User Register');
        },
        error: err => {
          if (err.status === 400 && err.error?.errors) {
            // Jeżeli serwer zwrócił błąd 400, to znaczy, że formularz nie przeszedł walidacji
            // Wyświetlamy więc użytkownikowi wszystkie błędy walidacji (zwracany jest słownik, który
            // pozwala określić, który input zawiera błąd i jaki jest ten błąd)
            Object.keys(err.error.errors).forEach(key => {
              err.error.errors[key].forEach((error: string) => {
                this.toastr.error(error, 'Validation Error');
              });
            });
          } else { // W przeciwnym wypadku wyświetlamy ogólny błąd
            this.toastr.error(err.error.error as string || 'An unknown error occurred', 'Error');
          }
        }
      })
  }
}
