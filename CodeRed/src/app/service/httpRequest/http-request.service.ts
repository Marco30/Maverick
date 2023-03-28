import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GenericHttpService } from '../GenericHttp/generic-http.service';

@Injectable({
  providedIn: 'root'
})
export class HttpRequestService {

  constructor( private genericHttpService: GenericHttpService,) { }

  public postData<T>(url: string, QueryParams: object = {}): Observable<any> {
    return this.genericHttpService.post<T>(url, QueryParams);
  }
  public getData<T>(url: string): Observable<any> {
    return this.genericHttpService.get<T>(url);
  }

  public deleteData<T>(url: string, item: any): Observable<any> {
    return this.genericHttpService.delete(url, item);
  }
}
