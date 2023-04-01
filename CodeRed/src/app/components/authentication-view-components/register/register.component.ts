import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Address } from 'src/app/model/address/address';
import { Register } from 'src/app/model/register/register';
import { AuthenticationService } from 'src/app/service/authentication/authentication.service';

enum ERRORS_TYPES {
  fullName = 'Full',
  email = 'email',
  city = 'city',
  street = 'street',
  zipCode = 'zipCode',
  matchedPasswords = 'passwordDontMatch',
  serverError = 'serverError',
}
enum ERRORS_MSGS {
  fullName = 'Full name is required',
  email = 'email name is required',
  city = 'city name is required',
  street = 'street name is required',
  zipCode = 'Zip code name is required',
  passowrdDontMatch = "Passwords don't match",
  serverError = 'Sorry, something went wrong',
}
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

  address: Address = {
    city: '',
    country: '',
    street: '',
    zipCode: '',
    attention: '',
    careOf: '',
  };

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
    address: this.address,
  };

  showTermsOfUse: boolean = false;
  noMatchPasswords: boolean = false;
  ERRORS_TYPES = ERRORS_TYPES;
  errorsMap: Map<ERRORS_TYPES, ERRORS_MSGS> = new Map();
  info: string = '';
  passwordConfirmation: string = '';
  cancelDataFetching: boolean = false;
  showLoading: boolean = false;
  submitted: boolean = false;

  checkSame() {
    const secondPassword = this.passwordConfirmation;
    const firstPassword = this.registerData.password;
    if (secondPassword !== firstPassword) {
      console.log('passwords not match');
      this.noMatchPasswords = true;
      if (this.registerform && this.registerform.controls) {
        this.registerform.controls['passwordConfirmationControl'].markAsDirty();
        this.registerform.controls['passwordConfirmationControl'].setErrors(
          null
        );
      }
    } else {
      // form control with errors
      this.noMatchPasswords = false;
      if (this.registerform && this.registerform.controls) {
        this.registerform.controls['passwordConfirmationControl'].setErrors({
          incorrect: true,
        });
        this.registerform.controls[
          'passwordConfirmationControl'
        ].markAsPristine();
        this.registerform.controls[
          'passwordConfirmationControl'
        ].markAsTouched();
      }
    }
  }

  removeError(type: ERRORS_TYPES) {
    this.errorsMap.delete(type);
  }

  register() {
    this.submitted = true;

    console.log('Registering with data:', this.registerData);
    if (this.registerData.password !== this.passwordConfirmation) {
      this.errorsMap.set(
        ERRORS_TYPES.matchedPasswords,
        ERRORS_MSGS.passowrdDontMatch
      );
    }
    if (!this.registerData.fullName) {
      this.errorsMap.set(ERRORS_TYPES.fullName, ERRORS_MSGS.fullName);
    }
    if (!this.registerData.email) {
      this.errorsMap.set(ERRORS_TYPES.email, ERRORS_MSGS.email);
    }
    if (!this.registerData.address.city) {
      this.errorsMap.set(ERRORS_TYPES.city, ERRORS_MSGS.city);
    }
    if (!this.registerData.address.street) {
      this.errorsMap.set(ERRORS_TYPES.street, ERRORS_MSGS.street);
    }
    if (!this.registerData.address.zipCode) {
      this.errorsMap.set(ERRORS_TYPES.zipCode, ERRORS_MSGS.zipCode);
    }
    if (this.errorsMap.size > 0) return;
    this.authenticationService.register(this.registerData).subscribe({
      next: (res) => {
        console.info('--------register------------');
        console.info(res);
        this.info = 'Congratulations! Your account has been registered';
        // Routing to login view won't work beacuse  the user is already at /login
      },
      error: (err) => {
        this.errorsMap.clear();
        this.errorsMap.set(ERRORS_TYPES.serverError, ERRORS_MSGS.serverError);
      },
    });
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
