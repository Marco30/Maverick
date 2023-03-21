import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainViewComponent } from './main-view.component';

const routes: Routes = [
  { path: '', component: MainViewComponent,
  children: [
    { path: 'ChatView', loadChildren: () => import('./views/chat-view/chat-view.module').then(m => m.ChatViewModule) },
    { path: '', redirectTo: "ChatView", pathMatch: "full" }
  ]
},
  {
    path: '**',
    redirectTo: 'ChatView'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainViewRoutingModule { }
