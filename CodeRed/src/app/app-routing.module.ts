import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/LoginView', pathMatch: 'full' },
  { path: 'MainView', loadChildren: () => import('./main-view/main-view.module').then(m => m.MainViewModule) },
  { path: 'LoginView', loadChildren: () => import('./login-view/login-view.module').then(m => m.LoginViewModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
