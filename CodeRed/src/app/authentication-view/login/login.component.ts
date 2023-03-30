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
  loginData = {
    socialSecurityNumber: '',
    email: '',
    password: '',
    error: '',
  };

  login(): void {
    console.log('Logging in with data:', this.loginData);
    // this.authenticationService.getAuthentication(this.loginData).subscribe({
    //   next: (res) => {
    //     if (!res.user) {
    //       this.loginData.error = 'Wrong credentials!';
    //     }
    //     this.authenticationService.setToken(res.token);
    //     console.info('-----login-----');
    //     console.info(res);
    //     // this.testService.toggleAuthentication(true);
    //     this.authenticationService.startCountingDown();
    //     console.info('parse token');
    //     // this.userService.setUser(res.user);
    //     this.router.navigate(['MainView/ChatView']);
    //   },
    //   error: () => {
    //     this.loginData.error = 'Wrong credentials';
    //   },
    // });
  }
}
