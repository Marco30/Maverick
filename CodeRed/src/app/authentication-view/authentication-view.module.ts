import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthenticationViewRoutingModule } from './authentication-view-routing.module';
import { AuthenticationViewComponent } from './authentication-view.component';
import { RegisterComponent } from '../components/authentication-view-components/register/register.component';
import { LoginComponent } from '../components/authentication-view-components/login/login.component';
import { ResetPasswordComponent } from '../components/authentication-view-components/reset-password/reset-password.component';
import { ResetPasswordRequestComponent } from '../components/authentication-view-components/reset-password-request/reset-password-request.component';
import { LoaderModule } from '../components/loader/loader.module';
import { ModalModule } from '../components/modal/modal.module';
import { FooterComponent } from '../components/footer/footer.component';

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
    ModalModule,
    FooterComponent,
  ],
})
export class AuthenticationViewModule {}
