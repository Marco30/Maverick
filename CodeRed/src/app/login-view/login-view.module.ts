import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginViewRoutingModule } from './login-view-routing.module';
import { LoginViewComponent } from './login-view.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    LoginViewComponent
  ],
  imports: [
    CommonModule,
    LoginViewRoutingModule,
    FormsModule
  ]
})
export class LoginViewModule { }
