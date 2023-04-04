import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Login } from 'src/app/model/login/login';
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
  @Input() sentLoginData: Login | null = null;
  showLoading: boolean = false;
  loginData: Login = {
    socialSecurityNumber: '',
    email: '',
    password: '',
  };
  error = '';

  ngOnInit(): void {
    if (this.sentLoginData) {
      this.loginData = this.sentLoginData;
    }
    console.log('login data from login component:', this.loginData);
  }
  login(): void {
    console.log('Logging in with data:', this.loginData);
    this.showLoading = true;
    // Validate empty fields
    // setTimeout(() => {
    this.authenticationService.login(this.loginData).subscribe({
      next: (res) => {
        this.showLoading = false;
        if (!res.user) {
          this.error = 'Wrong credentials!';
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
        this.error = 'Wrong credentials';
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
