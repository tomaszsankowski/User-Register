import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http"
import { environment } from '../../environments/environment.development';
import { UserDetails, UserDetailsDTO } from './user-details.model';
import { NgForm } from '@angular/forms';

// Klasa UserDetailsWithVisibility, która zawiera UserDetailsDTO oraz dodatkowe pole mówiące, czy chcemy wyświetlać szczegóły użytkownika
interface UserDetailsWithVisibility {
  userDetails: UserDetailsDTO;
  detailsInfo: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserDetailsService {

  // Adres URL API które obsługuje żądania dotyczące użytkowników
  url:string = environment.apiBaseUrl + '/Users';

  // Lista użytkowników
  list: UserDetailsWithVisibility[] = [];

  // Dane z formularza
  formData: UserDetails = new UserDetails();

  // Flagi (czy wysłano formularz, czy dodajemy nowego użytkownika)
  formSubmitted: boolean = false;
  isAddingNewUser: boolean = true;

  constructor(private http: HttpClient) {}

  // Funkcja aktualizuje listę użytkowników za pomocą żądania na serwer
  refreshList(){
    this.http.get(this.url)
    .subscribe({
      next: (res) => {
        // Przekształć odpowiedź na listę UserDetailsWithVisibility
        this.list = (res as UserDetails[]).map(userDetails => ({
          userDetails: userDetails,
          detailsInfo: false // Domyślnie ustawiamy detailsInfo na false
        }));
      },
      error: (err) => {
        console.log(err);
      }
    })
  }

  // Funkcja dodaje nowego użytkownika
  postUser() {
    // Wymagane jest dodanie sessionToken do nagłówka
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
      })
    };
    return this.http.post(this.url, this.formData, httpOptions);
  }

  // Funkcja aktualizuje wybranego użytkownika
  putUser() {
    // Wymagane jest dodanie sessionToken do nagłówka
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
      })
    };
    // zamiana UserDetails na UserDetailsDTO
    const userDetailsDTO: UserDetailsDTO = {
      id: this.formData.id,
      name: this.formData.name,
      surname: this.formData.surname,
      email: this.formData.email,
      phone: this.formData.phone,
      dateOfBirth: this.formData.dateOfBirth,
      category: this.formData.category,
      subcategory: this.formData.subcategory
    }
    return this.http.put(this.url + '/' + this.formData.id, userDetailsDTO, httpOptions);
  }

  // Funkcja usuwa wybranego użytkownika
  deleteUser(id: number) {
    // Wymagane jest dodanie sessionToken do nagłówka
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
      })
    };
    return this.http.delete(this.url + '/' + id, httpOptions);
  }

  // Funkcja resetuje formularz ustawiając mu domyślne wartości
  resetForm(form:NgForm){
    form.form.reset()
    this.formData = new UserDetails()
    this.formSubmitted = true;
  }
}
