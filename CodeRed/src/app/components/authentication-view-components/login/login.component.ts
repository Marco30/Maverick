import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/service/authentication/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  constructor(
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}
  showLoading: boolean = false;
  loginData = {
    socialSecurityNumber: '',
    email: '',
    password: '',
    error: '',
  };

  login(): void {
    console.log('Logging in with data:', this.loginData);
    this.showLoading = true;
    // Validate empty fields
    // setTimeout(() => {
    this.authenticationService.login(this.loginData).subscribe({
      next: (res) => {
        this.showLoading = false;
        if (!res.user) {
          this.loginData.error = 'Wrong credentials!';
          return;
        }
        console.info('-----login-----');
        console.info(res);
        console.info('parse token');
        // this.testService.toggleAuthentication(true);
        this.authenticationService.setToken(res.token);
        this.authenticationService.startCountingDown();
        this.authenticationService.setChatUser(res.user);
        this.router.navigate(['MainView/ChatView']);
      },
      error: () => {
        this.loginData.error = 'Wrong credentials';
        this.showLoading = false;
      },
      // complete: () => (this.showLoading = false),
    });
    // }, 1000);
  }

  cancelLoader() {
    this.showLoading = false;
  }
}
