import { Component } from '@angular/core';

@Component({
  selector: 'app-login-view',
  templateUrl: './login-view.component.html',
  styleUrls: ['./login-view.component.css']
})
export class LoginViewComponent {
  isDarkMode = false;
  showRegistrationForm = false;

  loginData = {
    email: '',
    password: ''
  };

  registerData = {
    name: '',
    email: '',
    password: '',
    password_confirmation: ''
  };

  constructor() { }

  ngOnInit() {
  }

  toggleDarkMode() {
    this.isDarkMode = !this.isDarkMode;
  }

  login() {
    console.log('Logging in with data:', this.loginData);
  }

  register() {
    console.log('Registering with data:', this.registerData);
  }


}
