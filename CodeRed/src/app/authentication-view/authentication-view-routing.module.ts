import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationViewComponent } from './authentication-view.component';

const routes: Routes = [{ path: '', component: AuthenticationViewComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthenticationViewRoutingModule {}
