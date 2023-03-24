import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/MainView', pathMatch: 'full' },
  { path: 'MainView', loadChildren: () => import('./main-view/main-view.module').then(m => m.MainViewModule) },
  { path: 'ChatView', loadChildren: () => import('./main-view/views/chat-view/chat-view.module').then(m => m.ChatViewModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
