import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HttpInterceptorService } from './service/httpInterceptor/http-interceptor.service';
import { ListSavedConversationsMenuComponent } from './components/main-view-components/list-saved-conversations-menu/list-saved-conversations-menu.component';


@NgModule({
  declarations: [AppComponent, ListSavedConversationsMenuComponent],
  imports: [BrowserModule, AppRoutingModule, NgbModule, HttpClientModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
