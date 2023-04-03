import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/service/authentication/authentication.service';

@Component({
  selector: 'app-reset-password-request',
  templateUrl: './reset-password-request.component.html',
  styleUrls: ['./reset-password-request.component.css'],
})
export class ResetPasswordRequestComponent {
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}

  resetPasswordData = {
    email: '',
    error: '',
    info: '',
  };
  showLoading: boolean = false;
  submitted: boolean = false;
  removeError() {
    this.resetPasswordData.error = '';
  }
  resetPasswordRequest() {
    this.submitted = true;
    console.log('reset password request submitted: ', this.resetPasswordData);

    const regex = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
    const match = regex.test(this.resetPasswordData.email);
    if (!match) {
      this.resetPasswordData.error = 'Email is not valid';
      return;
    }
    this.showLoading = true;

    console.log('reset password request submitted: ', this.resetPasswordData);

    this.resetPasswordData.error = '';
    this.authenticationService
      .sendResetPasswordRequest(this.resetPasswordData.email)
      .subscribe({
        next: (res) => {
          console.log('response from server: ', res);
          this.resetPasswordData.error = '';
          this.resetPasswordData.info = 'Check your email!';
          this.showLoading = false;
        },
        error: (err) => {
          this.resetPasswordData.error = 'Sorry, Something went wrong!';
          this.showLoading = false;
        },
      });
  }
}
