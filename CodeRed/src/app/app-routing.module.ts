import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './service/authGuard/auth-guard.service';

const routes: Routes = [
  { path: '', redirectTo: '/authView', pathMatch: 'full' },
  {
    path: 'MainView',
    loadChildren: () =>
      import('./main-view/main-view.module').then((m) => m.MainViewModule),
    canActivate: [AuthGuardService],
  },
  {
    path: 'authView',
    loadChildren: () =>
      import('./authentication-view/authentication-view.module').then(
        (m) => m.AuthenticationViewModule
      ),
  },
  {
    path: 'authView/reset/:token',
    loadChildren: () =>
      import('./authentication-view/authentication-view.module').then(
        (m) => m.AuthenticationViewModule
      ),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
