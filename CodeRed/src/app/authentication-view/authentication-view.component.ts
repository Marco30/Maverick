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
    if (this.router.url.includes('reset')) {
      this.currentView = VIEWS.resetPassword;
    }
  }
  setView(view: ViewsType) {
    if (this.currentView === VIEWS.resetPassword) {
      this.router.navigate(['authView']);
      return;
    }
    this.currentView = view;
  }
}
