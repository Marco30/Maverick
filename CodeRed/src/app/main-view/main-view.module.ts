import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainViewRoutingModule } from './main-view-routing.module';
import { MainViewComponent } from './main-view.component';
import { AiChatComponent } from '../components/ai-chat/ai-chat.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    MainViewComponent,
    AiChatComponent
  ],
  imports: [
    CommonModule,
    MainViewRoutingModule,
    FormsModule,
    HttpClientModule
  ]
})
export class MainViewModule { }
