import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChatViewRoutingModule } from './chat-view-routing.module';
import { ChatViewComponent } from './chat-view.component';
import { AiChatComponent } from 'src/app/components/ai-chat/ai-chat.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    ChatViewComponent,
    AiChatComponent
  ],
  imports: [
    CommonModule,
    ChatViewRoutingModule,
    FormsModule,
    HttpClientModule
  ]
})
export class ChatViewModule { }
