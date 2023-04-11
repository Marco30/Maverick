import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/service/authentication/authentication.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css'],
})
export class ResetPasswordComponent {
  @ViewChild('resetForm', { static: false }) resetForm!: NgForm;

  data: { password: string; confirmPassword: string } = {
    password: '',
    confirmPassword: '',
  };
  noMatchPasswords: boolean = false;
  success: boolean = false;
  error: string = '';
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute
  ) {}

  submitted: boolean = false;
  showLoading: boolean = false;
  toLogin() {
    this.router.navigate(['authView']);
  }
  checkSame(input: string) {
    const secondPassword = input;
    const firstPassword = this.data.password;
    if (secondPassword !== firstPassword) {
      this.noMatchPasswords = true;
    } else {
      this.noMatchPasswords = false;
    }
  }

  // Send the new password
  submitResetPassword() {
    this.submitted = true;
    if (
      this.noMatchPasswords ||
      !this.data.confirmPassword ||
      !this.data.password
    ) {
      return;
    }
    if (this.data.confirmPassword !== this.data.password) {
      this.error = 'Passwords do not match';
      return;
    }
    this.showLoading = true;
    this.error = '';
    this.success = false;
    const token = this.route.snapshot.params['token'];
    this.authenticationService
      .sendResetPassword(token, this.data.password)
      .subscribe({
        next: (res) => {
          this.success = true;
          this.showLoading = false;
        },
        error: (err) => {
          this.error = 'Sorry, Something went wrong!';
          this.showLoading = false;
        },
      });
    // this.router.navigate([PATHES.login]);
  }
}
