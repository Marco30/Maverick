import { Component } from '@angular/core';

const VIEWS = {
  login: 'login',
  register: 'register',
  forgotPassword: 'forgot-password',
} as const;

type Object<T> = T[keyof T];
type ViewsType = Object<typeof VIEWS>;

@Component({
  selector: 'app-login-view',
  templateUrl: './login-view.component.html',
  styleUrls: ['./login-view.component.css'],
})
export class LoginViewComponent {
  loginData = {
    email: '',
    password: '',
  };

  registerData = {
    name: '',
    email: '',
    password: '',
    password_confirmation: '',
  };

  email: string = '';
  VIEWS = VIEWS;
  currentView: ViewsType = this.VIEWS.login;

  constructor() {}

  setView(view: ViewsType) {
    this.currentView = view;
  }
  login() {
    console.log('Logging in with data:', this.loginData);
  }

  register() {
    console.log('Registering with data:', this.registerData);
  }
  resetPassword() {
    console.log('reset password: ', this.email);
  }
}
