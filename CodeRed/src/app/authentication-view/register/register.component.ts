import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/service/authentication/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}
  registerData = {
    name: '',
    email: '',
    password: '',
    password_confirmation: '',
    error: [],
  };
  register() {
    console.log('Registering with data:', this.registerData);
  }
}
