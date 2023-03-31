import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Register } from 'src/app/model/register/register';
import { AuthenticationService } from 'src/app/service/authentication/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  @ViewChild('registerform', { static: false }) registerform!: NgForm;
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}
  registerData: Register = {
    socialSecurityNumber: '',
    fullName: '',
    firstName: '',
    lastName: '',
    agreeMarketing: true,
    subscribeToEmailNotification: true,
    email: '',
    password: '',
    gender: '',
    birthDay: 0,
    birthMonth: 0,
    birthYear: 0,
    address: {
      city: '',
      street: '',
      zipCode: '',
      country: '',
    },
  };
  showTermsOfUse: boolean = false;
  noMatchPasswords: boolean = false;
  errors: string[] = [];
  passwordConfirmation: string = '';
  cancelDataFetching: boolean = false;
  showLoading: boolean = false;

  checkSame(input: string) {
    const secondPassword = input;
    const firstPassword = this.registerData.password;
    if (secondPassword !== firstPassword) {
      // stuff for form control
      console.log('passwords not match');

      this.noMatchPasswords = true;
      this.registerform.controls?.['passwordConfirmation'].markAsDirty();
      this.registerform.form.controls?.['passwordConfirmation'].setErrors(null);
    } else {
      // form control with errors
      this.noMatchPasswords = false;
      this.registerform.form.controls?.['passwordConfirmation'].setErrors({
        incorrect: true,
      });
      this.registerform.form.controls?.[
        'passwordConfirmation'
      ].markAsPristine();
      this.registerform.form.controls?.['passwordConfirmation'].markAsTouched();
    }
  }

  register() {
    console.log('Registering with data:', this.registerData);
    if (this.registerData.password !== this.passwordConfirmation) {
      this.errors.push('Passwords do not match');
    }
    if (!this.registerData.fullName) {
      this.errors.push('Full name is required');
    }
    if (!this.registerData) if (this.errors.length > 0) return;
    // this.authenticationService.register(this.registerData).subscribe({
    //   next: (res) => {
    //     console.info('--------register------------');
    //     console.info(res);
    //     // Routing to login view won't work beacuse  the user is already at /login
    //   },
    //   error: (err) => {
    //     this.errors = [];
    //     this.errors.push('Error registering user');
    //   },
    // });
  }
  getUserData(): void {
    let num = this.registerData.socialSecurityNumber;
    if (this.registerData.socialSecurityNumber.includes('-')) {
      num = this.registerData.socialSecurityNumber.replace('-', '');
    }
    if (num.length == 12) {
      this.cancelDataFetching = false;
      this.showLoading = true;
      this.registerData.socialSecurityNumber = num;
      this.authenticationService
        .getUserDataFromSecurityNumber(this.registerData)
        .subscribe({
          next: (userData) => {
            if (this.cancelDataFetching) return;
            console.log('uploaded image to server, event: ', userData);
            const { year, month, day } = this.getBirthDay(
              this.registerData.socialSecurityNumber
            );
            this.registerData.fullName =
              userData.fullName || this.registerData.fullName;
            this.registerData.firstName =
              userData?.firstName || this.registerData.firstName;
            this.registerData.lastName =
              userData?.lastName || this.registerData.lastName;
            this.registerData.address.city =
              userData?.address?.city || this.registerData.address.city;
            this.registerData.address.street =
              userData?.address?.street || this.registerData.address.street;
            this.registerData.address.zipCode =
              userData?.address?.zipCode || this.registerData.address.zipCode;
            this.registerData.gender =
              userData?.gender || this.registerData.gender;
            this.registerData.birthYear = Number(year);
            this.registerData.birthMonth = Number(month);
            this.registerData.birthDay = Number(day);
            this.showLoading = false;
          },
          error: (err) => {
            console.log(err);
            this.showLoading = false;
          },
        });
    }
  }

  getBirthDay(socialSecurityNumber: string) {
    console.log('socialSecurityNumber: ', socialSecurityNumber);
    const bd = socialSecurityNumber.slice(0, 8);
    const year = bd.slice(0, 4);
    const month = bd.slice(4, 6);
    const day = bd.slice(6, 8);
    return { year, month, day };
  }

  cancelLoader() {
    this.cancelDataFetching = true;
    this.showLoading = false;
  }
  public onAgreeMarketingChanged(value: boolean) {
    this.registerData.agreeMarketing = value;
  }
  public onSubscribeToEmailChanged(value: boolean) {
    this.registerData.subscribeToEmailNotification = value;
  }

  closeModal() {
    this.showTermsOfUse = false;
  }
}
