import { Injectable } from '@angular/core';
import { AuthenticationService } from '../authentication/authentication.service';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable, catchError, retry, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpInterceptorService {


   
  constructor(private authenticationService: AuthenticationService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // All HTTP requests are going to go through this method

    // console.log("INTERCEPTOR");
    // We retrieve the token, if any
    const token = this.authenticationService.getToken();
    let newHeaders = req.headers;
    if (token) {
      // If we have a token, we append it to our new headers
      newHeaders = newHeaders.append("bearer", token);
      newHeaders = newHeaders.append("authorization", `Bearer ${token}`);
    }
    console.info('Token test nu');
    console.info(token);
    // Finally we have to clone our request with our new headers
    // This is required because HttpRequests are immutable
    const authReq = req.clone({ headers: newHeaders }); // Then we return an Observable that will run the request
    // or pass it to the next interceptor if any
    return next.handle(authReq).pipe(
      retry(1),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => this.handleError(error));
      })
    );
  }

  handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else if (error.status === 401) {
      // unauthorized error
      errorMessage = 'Unauthorized';
    } else if (error.status === 415) {
      // unsupported media type error
      errorMessage = 'Unsupported Media Type';
    } else if (error.status === 500) {
      // internal server error
      errorMessage = 'Internal Server Error';
    } else if (error.status === 404) {
      // not found error
      errorMessage = 'Not Found';
    } else {
      // server-side error
      const errorObj = error.error;
      errorMessage = `Error: ${JSON.stringify(errorObj)}`;
    }
    window.alert(errorMessage);
    return throwError(() => errorMessage);
  }

  
}
