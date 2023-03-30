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
  info: string = '';
  error: string = '';
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private route: ActivatedRoute
  ) {}

  toLogin() {
    this.router.navigate(['auth']);
  }
  checkSame(input: string) {
    this.info = '';
    const secondPassword = input;
    const firstPassword = this.data.password;
    if (secondPassword !== firstPassword) {
      // stuff for form control
      this.noMatchPasswords = true;
      this.resetForm.controls?.['confirmPassword'].markAsDirty();
      this.resetForm.form.controls?.['confirmPassword'].setErrors(null);
    } else {
      // form control with errors
      this.noMatchPasswords = false;
      this.resetForm.form.controls?.['confirmPassword'].setErrors({
        incorrect: true,
      });
      this.resetForm.form.controls?.['confirmPassword'].markAsPristine();
    }
  }

  // Send the new password
  submitResetPassword() {
    if (
      this.noMatchPasswords ||
      !this.data.confirmPassword ||
      !this.data.password
    ) {
      return;
    }
    const token = this.route.snapshot.params['token'];
    console.log('resetting password: ', this);
    console.log('params: ', token);
    this.authenticationService
      .sendResetPassword(token, this.data.password)
      .subscribe({
        next: (res) => {
          console.log('success resetting password:', res);
          this.info = 'Success resettign password';
          // this.router.navigate(['auth']);
        },
        error: (err) => {
          console.log('error resetting password: ', err);
          this.error = 'Sorry, Something went wrong!';
        },
      });
    // this.router.navigate([PATHES.login]);
  }
}
