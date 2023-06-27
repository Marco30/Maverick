import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GenericHttpService } from '../genericHttp/generic-http.service';

@Injectable({
  providedIn: 'root'
})
export class HttpRequestService {

  constructor( private genericHttpService: GenericHttpService,) { }

  public postData<T>(url: string, QueryParams: object = {}): Observable<any> {
    return this.genericHttpService.post<T>(url, QueryParams);
  }
  public getData<T>(url: string, QueryParams: object = {}): Observable<any> {
    return this.genericHttpService.get<T>(url, QueryParams);
  }

  public deleteData<T>(url: string, item: any): Observable<any> {
    return this.genericHttpService.delete(url, item);
  }
  public deleteData1<T>(url: string,QueryParams: object = {}): Observable<any> {
    return this.genericHttpService.delete1(url, QueryParams);
  }

  public deleteData2<T>(url: string): Observable<any> {
    return this.genericHttpService.delete2<T>(url);
  }
}
