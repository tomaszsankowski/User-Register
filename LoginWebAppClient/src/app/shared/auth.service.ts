import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { UserLogin } from './user-login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isLoggedIn: boolean = false;
  sessionToken: string = '';
  loggedEmail: string ='';

  // Adres url strony serwera
  url:string = environment.apiBaseUrl + '/Users';

  // Dane formularza
  formData: UserLogin = new UserLogin();

  constructor(private http: HttpClient) { }

  // Funkcja obsługująca żądanie logowania
  login(){
    return this.http.post(this.url + '/Login', this.formData)
  }

  // Fubkcja obsługująca żądanie wylogowania (wysyła sessionToken w nagłówku)
  logout(){
    this.isLoggedIn = false;
    //Dodanie sessionToken do nagłówka
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
      })
    };
    return this.http.post(this.url + '/Logout', this.formData, httpOptions)
  }
}