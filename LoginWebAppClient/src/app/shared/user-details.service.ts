import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http"
import { environment } from '../../environments/environment.development';
import { UserDetails, UserDetailsDTO } from './user-details.model';
import { NgForm } from '@angular/forms';

interface UserDetailsWithVisibility {
  userDetails: UserDetailsDTO;
  detailsInfo: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class UserDetailsService {

  // This is the URL of the API that we want to call
  url:string = environment.apiBaseUrl + '/Users';

  // List of Users
  list: UserDetailsWithVisibility[] = [];

  // Data from form
  formData: UserDetails = new UserDetails();
  formSubmitted: boolean = false;

  isAddingNewUser: boolean = true;

  // Constructor
  constructor(private http: HttpClient) {}

  // Function that displays all the users
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

  // Function that creates new user
  postUser() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
      })
    };
    return this.http.post(this.url, this.formData, httpOptions);
  }

  putUser() {
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

  deleteUser(id: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${sessionStorage.getItem('sessionToken')}`
      })
    };
    return this.http.delete(this.url + '/' + id, httpOptions);
  }

  resetForm(form:NgForm){
    form.form.reset()
    this.formData = new UserDetails()
    this.formSubmitted = true;
  }
}
