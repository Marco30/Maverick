import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, Subject, timer } from 'rxjs';
import { AuthToken } from 'src/app/model/authToken/auth-token';
import { Login } from 'src/app/model/login/login';
import { Register } from 'src/app/model/register/register';
import { User } from 'src/app/model/user/user';
import { environment } from 'src/environments/environment';
import { GenericHttpService } from '../genericHttp/generic-http.service';
import { HttpRequestService } from '../httpRequest/http-request.service';
import { UserData } from 'src/app/model/userdata/user-data';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  constructor(
    private httpRequestService: HttpRequestService,
    private router: Router // public dialogService: DialogService
  ) {}

  chatUser: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(
    null
  );
  getChatUser = () => this.chatUser.asObservable();
  private tokenName = 'jwt_token';
  private timerSubscription: any;
  private reminderSubscription: any;
  private refreshSubscription: any;
  private refreshJwtTokenSubscription: any;
  private logoutPath = '/authView';

  // private chatUser: any;
  setChatUser(user: User) {
    this.chatUser.next(user);
  }
  public login(logindata: Login): Observable<AuthToken> {
    console.info('login data: ', logindata);

    /*const url = environment.authenticate_url;

    return this.genericHttpService.post<any>(url, logindata);*/

    const url = environment.authenticate_url;

    const queryParams = { model: logindata };

    return this.httpRequestService.postData<any>(url, queryParams);
  }

  register(registerData: Register) {
    const url = environment.register_url;
    const queryParams = { model: registerData };
    return this.httpRequestService.postData<Register>(url, queryParams);
  }

  // change any to userdata
  getUserDataFromSecurityNumber(userInfo: Register): Observable<UserData> {
    const url = environment.get_user_data_from_security_number;
    const queryParams = { model: userInfo };
    return this.httpRequestService.postData<any>(url, queryParams);
  }

  sendResetPasswordRequest(email: string) {
    const url = environment.reset_password_request;
    const queryParams = { model: { email } };
    return this.httpRequestService.postData(url, queryParams);
  }

  sendResetPassword(token: string, password: string) {
    const url = environment.reset_password;
    const queryParams = { model: { Password: password, Token: token } };
    return this.httpRequestService.postData(url, queryParams);
  }

  sendTest() {
    const queryParams = { model: { Password: 'password', Token: 'token' } };
    return this.httpRequestService.postData<any>(
      'https://localhost:7125/wargames/test',
      queryParams
    );
  }

  logout() {
    localStorage.removeItem('chatId');
    this.cleanTokenData();
    this.chatUser.next(null);
    location.reload();
    this.router.navigate([this.logoutPath]);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenName);
  }

  setToken(token: string): void {
    localStorage.setItem(this.tokenName, token);
  }

  deleteToken() {
    localStorage.removeItem(this.tokenName);
    // this.userService.setUserToNull();
  }

  getTokenExpirationDate(): Date {
    const token = this.getToken();
    const decoded = this.parseJwt(token);

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  isTokenExpired(): boolean {
    const token = this.getToken();

    if (!token) {
      return true;
    }

    const date = this.getTokenExpirationDate();
    if (date === undefined) {
      return false;
    }
    return !(date!.valueOf() > new Date().valueOf());
  }

  public getNewtoken(): Observable<AuthToken> {
    const url = environment.refresh_token_url;

    //return this.genericHttpService.post<any>(url);
    return this.httpRequestService.postData<any>(url);
  }

  /**
   * post data.
   */


  public parseJwt(token: any) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );

    return JSON.parse(jsonPayload);
  }

  getDataFromToken(dataName: string) {
    const token = this.getToken();
    const decoded = this.parseJwt(token);

    return decoded[dataName];
  }

  // displays message before logging out as well, check if users are active and renews the token if user is aktiv

  startCountingDown() {
    console.info('token');
    console.info(this.getToken);

    // review this feature maybe better to use an Interval timer that check every 20 seconds if token valid
    const expirationDate = this.getTokenExpirationDate();

    console.info('date of token expiration ' + expirationDate);

    const remainingTime = expirationDate.getTime() - Date.now();

    const reminderOfTokenExpiration = remainingTime - 300000; // 5 min

    const refreshToken = remainingTime - 600000; // 10 min

    console.info('refresh Token if active ' + refreshToken);

    console.info('reminder Of Token Expiration ' + reminderOfTokenExpiration);

    console.info('remainingTime ' + remainingTime);

    this.refreshSubscription = timer(refreshToken).subscribe(() =>
      this.checkIfUserActive()
    );

    this.reminderSubscription = timer(reminderOfTokenExpiration).subscribe(() =>
      this.Reminder()
    );

    this.timerSubscription = timer(remainingTime).subscribe(() =>
      this.tokenHasExpired()
    );
  }

  checkIfUserActive() {
    this.refreshJwtTokenSubscription = this.RefreshJwtToken.bind(this);

    document.body.addEventListener('click', this.refreshJwtTokenSubscription);
    document.body.addEventListener('keydown', this.refreshJwtTokenSubscription);
  }

  RefreshJwtToken() {
    console.info('------Refresh Token--------');

    let newToken: AuthToken;

    this.getNewtoken().subscribe((res) => {
      const newToken = res.token;

      this.cleanTokenData();

      this.setToken(newToken);

      this.startCountingDown();
      console.info('------set new Token--------');
    });
  }

  cleanTokenData() {
    document.body.removeEventListener(
      'click',
      this.refreshJwtTokenSubscription
    );
    document.body.removeEventListener(
      'keydown',
      this.refreshJwtTokenSubscription
    );
    this.deleteToken();
    if (this.refreshSubscription) {
      this.refreshSubscription.unsubscribe();
      this.reminderSubscription.unsubscribe();
      this.timerSubscription.unsubscribe();
    }
  }

  Reminder() {
    console.info('Token will expir in 5 min');
    const expirationDate = this.getTokenExpirationDate();
    this.reminderSubscription.unsubscribe();
    console.info(
      'Token will expir in 5 min ' +
        expirationDate.toString() +
        ', Time now ' +
        Date.now().toString()
    );
    /* const ref = this.dialogService.open(ConfirmComponent, {
      data: {
        title: "Påminnelse",
        message: "Du har varit inaktiv, om 5 minuter loggas du ut!",
        firstButton: "Fortsätt var inloggad",
        secondButton: "Logga ut",
      },
    });

    ref.afterClosed.subscribe((result) => {
      if (result) {
        this.RefreshJwtToken();
      } else {
        this.logout();
      }
    }); */
  }

  tokenHasExpired() {
    if (this.isTokenExpired()) {
      console.info('Token has expired');
      const expirationDate = this.getTokenExpirationDate();
      console.info(
        'Token has expired ' +
          expirationDate.toString() +
          ', Time now ' +
          Date.now().toString()
      );
      // alert('Token has expired ' + expirationDate.toString() + ', Time now ' + Date.now().toString());
      this.cleanTokenData();
      this.router.navigate([this.logoutPath]);
    } else {
      console.info('error in tokenIsExpired func');
    }
  }
}
