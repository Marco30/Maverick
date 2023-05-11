import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChatViewRoutingModule } from './chat-view-routing.module';
import { ChatViewComponent } from './chat-view.component';

import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AiChatComponent } from 'src/app/components/chat-view-components/ai-chat/ai-chat.component';
import { InformationComponent } from 'src/app/components/chat-view-components/information/information.component';


@NgModule({
  declarations: [
    ChatViewComponent,
    AiChatComponent,
    InformationComponent
  ],
  imports: [
    CommonModule,
    ChatViewRoutingModule,
    FormsModule,
    HttpClientModule
  ]
})
export class ChatViewModule { }
