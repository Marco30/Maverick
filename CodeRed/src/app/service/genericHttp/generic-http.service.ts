import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GenericHttpService {

  constructor(private httpClient: HttpClient) {}

  post<T>(url: string, params: object = {}): Observable<T> {

    let httQueryParams: object;
  
    if (this.checkIfParams(params)) { 
      httQueryParams = this.addParamsToHttp(params);
    
    } else {
        
      httQueryParams = this.getModel(params);
    }
    return this.httpClient.post<T>(url, httQueryParams);
  }

  get<T>(url: string, params: object = {}): Observable<T> {
    
    let httQueryParams: object;
    if (this.checkIfParams(params)) {

      httQueryParams = this.addParamsToHttp(params);
    } else {
      httQueryParams = this.getModel(params);
    }
    return this.httpClient.get<T>(url, httQueryParams);
  }

  addParamsToHttp(paramsList: any): object {
    let params = new HttpParams();

    for (let key in paramsList) {
      params = params.append(key, paramsList[key]);
    }

    const httQueryParams = { params: params };

    return httQueryParams;
  }

  checkIfParams(paramsList: any) {
    for (let key in paramsList) {
      if (key === "model") {
        return false;
      }
    }

    return true;
  }

  getModel(paramsList: any) {

    for (let key in paramsList) {
      if (key === "model") {

        return paramsList[key];
      }
    }
    return null;
  }

  delete<T>(url: string, item: any) {
    return this.httpClient.delete(url, item);
  }

  delete1<T>(url: string, params: object = {}): Observable<T> {

    let httQueryParams: object;
  
    if (this.checkIfParams(params)) { 
      httQueryParams = this.addParamsToHttp(params);
    
    } else {
      httQueryParams = this.getModel(params);
    }
    
console.info('options test');
console.info(httQueryParams);
    return this.httpClient.delete<T>(url, httQueryParams);
  }





}
