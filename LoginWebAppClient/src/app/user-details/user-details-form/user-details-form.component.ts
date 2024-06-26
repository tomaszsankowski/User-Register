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

  onSubmit(form:NgForm) {

    if(this.service.formData.category == 'Prywatny')
    {
      this.service.formData.subcategory = null;
    }
    this.service.formSubmitted = true;

    // Check if the form is valid
    if(form.valid == false)
    {
      return
    }

    if(this.service.formData.id == 0)
    {
      this.insertRecord(form)
    }
    else
    {
      this.updateRecord(form)
    }
  }

  insertRecord(form: NgForm) {
    this.service.postUser()
      .subscribe({
        next: res => {
          this.service.list = (res as UserDetails[]).map(userDetails => ({
            userDetails: userDetails,
            detailsInfo: false
          }));
          this.service.resetForm(form);
          this.toastr.success('Inserted successfully', 'User Register');
        },
        error: err => {
          if (err.status === 400 && err.error?.errors) {
            Object.keys(err.error.errors).forEach(key => {
              err.error.errors[key].forEach((error: string) => {
                this.toastr.error(error, 'Validation Error');
              });
            });
          } else {
            this.toastr.error(err.error.error as string || 'An unknown error occurred', 'Error');
          }
        }
      })
  }
  
  updateRecord(form: NgForm) {
    this.service.isAddingNewUser = true;
    this.service.putUser()
      .subscribe({
        next: res => {
          this.service.list = (res as UserDetailsDTO[]).map(userDetails => ({
            userDetails: userDetails,
            detailsInfo: false
          }));
          this.service.resetForm(form);
          this.toastr.info('Updated successfully', 'User Register');
        },
        error: err => {
          if (err.status === 400 && err.error?.errors) {
            Object.keys(err.error.errors).forEach(key => {
              err.error.errors[key].forEach((error: string) => {
                this.toastr.error(error, 'Validation Error');
              });
            });
          } else {
            this.toastr.error(err.error.error as string || 'An unknown error occurred', 'Error');
          }
        }
      })
  }
}
