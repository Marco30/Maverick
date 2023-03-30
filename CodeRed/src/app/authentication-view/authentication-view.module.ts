import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthenticationViewRoutingModule } from './authentication-view-routing.module';
import { AuthenticationViewComponent } from './authentication-view.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ResetPasswordRequestComponent } from './reset-password-request/reset-password-request.component';
import { LoaderModule } from '../components/loader/loader.module';

@NgModule({
  declarations: [
    AuthenticationViewComponent,
    RegisterComponent,
    LoginComponent,
    ResetPasswordComponent,
    ResetPasswordRequestComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    AuthenticationViewRoutingModule,
    LoaderModule,
  ],
})
export class AuthenticationViewModule {}
