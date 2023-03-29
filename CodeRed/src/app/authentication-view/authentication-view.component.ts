import { Component } from '@angular/core';
import { Router } from '@angular/router';
const VIEWS = {
  login: 'login',
  register: 'register',
  resetPassword: 'forgot-password',
  resetPasswordRequest: 'reset-password-request',
} as const;
type Object<T> = T[keyof T];
type ViewsType = Object<typeof VIEWS>;
@Component({
  selector: 'app-authentication-view',
  templateUrl: './authentication-view.component.html',
  styleUrls: ['./authentication-view.component.css'],
})
export class AuthenticationViewComponent {
  VIEWS = VIEWS;
  currentView: ViewsType = this.VIEWS.login;
  constructor(private router: Router) {
    console.log('url: ', this.router.url);
  }
  setView(view: ViewsType) {
    this.currentView = view;
  }
}
