import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../authentication/authentication.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardService {
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {}

  canActivate() {
    if (!this.authenticationService.isTokenExpired()) {
      return true;
    }
    this.authenticationService.cleanTokenData();
    this.router.navigate(['authView']);

    return false;
  }
}
