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

  // This is the URL of the API that we want to call
  url:string = environment.apiBaseUrl + '/Users';

  formData: UserLogin = new UserLogin();

  constructor(private http: HttpClient) { }

  login(){
    return this.http.post(this.url + '/Login', this.formData)
  }

  logout(){
    this.isLoggedIn = false;
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
      })
    };
    return this.http.post(this.url + '/Logout', this.formData, httpOptions)
  }
}