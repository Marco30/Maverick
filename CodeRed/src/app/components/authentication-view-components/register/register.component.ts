import { Component, ViewChild, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Address } from 'src/app/model/address/address';
import { Login } from 'src/app/model/login/login';
import { GENDERS, Register } from 'src/app/model/register/register';
import { AuthenticationService } from 'src/app/service/authentication/authentication.service';

enum ERRORS_TYPES {
  fullName = 'Full',
  firstName = 'firstName',
  lastName = 'lastName',
  email = 'email',
  city = 'city',
  street = 'street',
  zipCode = 'zipCode',
  matchedPasswords = 'passwordDontMatch',
  serverError = 'serverError',
  dateOfBirth = 'dateOfBirth',
  gender = 'gender',
  mobile = 'mobile',
}
enum ERRORS_MSGS {
  fullName = 'Full name is required',
  firstName = 'First name is required',
  lastName = 'Last name is required',
  email = 'email name is required',
  city = 'city name is required',
  street = 'street name is required',
  zipCode = 'Zip code name is required',
  passowrdDontMatch = "Passwords don't match",
  serverError = 'Sorry, something went wrong',
  dateOfBirth = 'Birth Date is required',
  gender = 'Gender is required',
  mobile = 'Mobile number is required',
}
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  @ViewChild('registerform', { static: false }) registerform!: NgForm;
  @Output() success: EventEmitter<Login> = new EventEmitter();
  constructor(private authenticationService: AuthenticationService) {}

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
    gender: GENDERS.other,
    dateOfBirth: new Date('2010-01-16'),
    phoneNumber: undefined,
    mobilePhoneNumber: undefined,
    address: this.address,
  };

  // name regex No numbers, minimum two letter, no leading or trailing white spaces
  nameRegex = /^[A-Za-zÀ-ÖØ-öø-ÿ]{2,}(?:\s+[A-Za-zÀ-ÖØ-öø-ÿ]+)*$/;
  // email regex
  emailRegex = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
  // personal security number
  securityNumberRegex = /^(19|20)?(\d{6}([-+]|\s)\d{4}|(?!19|20)\d{10})$/;
  // phone number regex, allows internationals phone numbers, wrong phone number might pass
  phoneRegex = /^\+?\d{1,3}[-.\s]?\d{3,4}[-.\s]?\d{4,6}$/;
  GENDERS = GENDERS;
  showLoading: boolean = false;
  showTermsOfUse: boolean = false;
  noMatchPasswords: boolean = false;
  ERRORS_TYPES = ERRORS_TYPES;
  errorsMap: Map<ERRORS_TYPES, ERRORS_MSGS> = new Map();
  info: string = '';
  passwordConfirmation: string = '';
  cancelDataFetching: boolean = false;
  submitted: boolean = false;
  serverError: string = '';
  successRegistration: boolean = false;
  isFetchingUserData: boolean = false;
  checkSame() {
    const secondPassword = this.passwordConfirmation;
    const firstPassword = this.registerData.password;
    if (secondPassword !== firstPassword) {
      console.log('passwords not match');
      this.noMatchPasswords = true;
      console.log('this line is working outside if statment');
      if (this.registerform && this.registerform.controls) {
        console.log('this line is working inside if statement');

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
    this.serverError = '';
  }

  register() {
    console.log('register date: ', this.registerData.dateOfBirth);

    this.submitted = true;

    console.log('Registering with data:', this.registerData);
    if (this.registerData.password !== this.passwordConfirmation) {
      this.errorsMap.set(
        ERRORS_TYPES.matchedPasswords,
        ERRORS_MSGS.passowrdDontMatch
      );
    }
    const firstNameValide = this.nameRegex.test(this.registerData.firstName);
    const lastNameValide = this.nameRegex.test(this.registerData.lastName);
    if (!this.registerData.firstName || !firstNameValide) {
      this.errorsMap.set(ERRORS_TYPES.firstName, ERRORS_MSGS.firstName);
    }
    if (!this.registerData.lastName || !lastNameValide) {
      this.errorsMap.set(ERRORS_TYPES.lastName, ERRORS_MSGS.lastName);
    }
    if (!this.registerData.dateOfBirth) {
      this.errorsMap.set(ERRORS_TYPES.dateOfBirth, ERRORS_MSGS.dateOfBirth);
    }
    if (!this.registerData.gender) {
      this.errorsMap.set(ERRORS_TYPES.gender, ERRORS_MSGS.gender);
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
    console.log('register data: ', this.registerData);

    this.showLoading = true;
    this.isFetchingUserData = false;
    this.authenticationService.register(this.registerData).subscribe({
      next: (res) => {
        console.info('--------register------------');
        console.info(res);
        this.successRegistration = true;
        this.showLoading = false;
      },
      error: (err) => {
        this.errorsMap.clear();
        this.serverError = 'Sorry, something went wrog!';
        this.showLoading = false;
      },
    });
  }

  getUserData(): void {
    let num = this.registerData.socialSecurityNumber;
    if (this.registerData.socialSecurityNumber.includes('-')) {
      num = this.registerData.socialSecurityNumber.replace('-', '');
    }
    if (num.length == 12) {
      this.isFetchingUserData = true;
      this.showLoading = true;
      this.registerData.socialSecurityNumber = num;
      this.authenticationService
        .getUserDataFromSecurityNumber(this.registerData)
        .subscribe({
          next: (userData) => {
            if (this.cancelDataFetching) return;
            console.log('uploaded image to server, event: ', userData);
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
            this.registerData.dateOfBirth = this.getDateOfBirth(
              this.registerData.socialSecurityNumber
            );
            this.showLoading = false;
          },
          error: (err) => {
            if (this.cancelDataFetching) return;
            console.log(err);
            // Show erros from backend, Avoid revealign existing emails
            this.serverError = 'Sorry, something went wrong!';
            this.showLoading = false;
          },
        });
    }
  }

  getDateOfBirth(socialSecurityNumber: string) {
    console.log('socialSecurityNumber: ', socialSecurityNumber);
    const bd = socialSecurityNumber.slice(0, 8);
    const year = bd.slice(0, 4);
    const month = bd.slice(4, 6);
    const day = bd.slice(6, 8);
    return new Date(year + '-' + month + '-' + day);
  }

  abortUserDataFetching() {
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

  toLogin() {
    console.log('To login');
    this.success.emit({
      socialSecurityNumber: '',
      email: this.registerData.email,
      password: this.registerData.password,
    });
  }
}
