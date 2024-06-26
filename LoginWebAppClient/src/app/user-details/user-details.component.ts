import { Component, OnInit } from '@angular/core';
import { UserDetailsFormComponent } from './user-details-form/user-details-form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { UserDetailsService } from '../shared/user-details.service';
import { AuthService } from '../shared/auth.service';
import { CommonModule } from '@angular/common';
import { UserDetails, UserDetailsDTO } from '../shared/user-details.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-details',
  standalone: true,
  imports: [UserDetailsFormComponent, LoginFormComponent, CommonModule],
  templateUrl: './user-details.component.html',
  styles: ``
})
export class UserDetailsComponent implements OnInit{

  constructor(public service: UserDetailsService, public authService: AuthService, private toastr:ToastrService) {

  }
  ngOnInit(): void {
    this.service.refreshList();
  }

  // Funkcja przenosząca dane użytkownika do formualrza (elementu service.formData), pozwalająca na jego modyfikację
  populateForm(selectedRecord: UserDetailsDTO){
    //zamiana UserDetailsDTO na UserDetails
    const userDetails: UserDetails = {
      id: selectedRecord.id,
      name: selectedRecord.name,
      surname: selectedRecord.surname,
      password: '',
      email: selectedRecord.email,
      phone: selectedRecord.phone,
      dateOfBirth: selectedRecord.dateOfBirth,
      category: selectedRecord.category,
      subcategory: selectedRecord.subcategory
    }
    this.service.formData = Object.assign({}, userDetails);
    this.service.isAddingNewUser = false;
  }

  // Obsługa guzika usuwającego klikniętego użytkownika
  onDelete(id:number){
    // Wyświetlenie okna potwierdzającego usunięcie
    if(confirm('Are you sure to delete this record?'))
    {
      // Usunięcie użytkownika, a bardziej zlecenie tego usunięcia serwisowi
      this.service.deleteUser(id)
      .subscribe({
        next: res => {
          this.service.refreshList();
          this.toastr.error('Deleted successfully', 'User Register')
        },
        error: err => {
          console.log(err);
        }
      })
    }
  }
}