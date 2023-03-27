import { Component } from '@angular/core';
import { ThemeService } from '../services/theme.service';

@Component({
  selector: 'app-login-view',
  templateUrl: './login-view.component.html',
  styleUrls: ['./login-view.component.css'],
})
export class LoginViewComponent {
  showRegistrationForm = false;

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

  constructor(private themeService: ThemeService) {}

  ngOnInit() {}

  toggleDarkMode() {
    this.themeService.toggleDarkTheme();
  }

  login() {
    console.log('Logging in with data:', this.loginData);
  }

  register() {
    console.log('Registering with data:', this.registerData);
  }
}
