import { Injectable } from '@angular/core';
import { HttpRequestService } from '../httpRequest/http-request.service';
import { Question } from 'src/app/model/question/question';
import { environment } from 'src/environments/environment';
import { Answer } from 'src/app/model/answer/answer';
import { Observable } from 'rxjs';
import { Conversation } from 'src/app/model/conversation/conversation';
import { Conversations } from 'src/app/model/conversations/conversations';
import { ConversationTree } from 'src/app/model/conversationTree/conversation-tree';
import { ConversationInfo } from 'src/app/model/conversationInfo/conversation-info';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(private httpRequestService: HttpRequestService,) { }

  askTheAI(questionData: Question) : Observable<Answer>{
    const url = environment.ask_question;
    const queryParams = { model: questionData};
    return this.httpRequestService.postData<Question>(url, queryParams);
  }

  changeConversationName(conversationInfo: ConversationInfo) : Observable<any>{
    const url = environment.changeconversation_name;
    console.info('conversationInfo namn')
    console.info(conversationInfo.name)
    let conversationObject = { conversationId: conversationInfo.id, newName: conversationInfo.name};
    const queryParams = { model: conversationObject};
    return this.httpRequestService.postData<any>(url, queryParams);
  }


  conversation(conversationId: number) : Observable<ConversationTree>{
    const url = environment.get_conversation;
    console.info('conversationId')
    console.info(conversationId)
    let conversationObject = { conversationId: conversationId};
    const queryParams = { model: conversationObject};
    return this.httpRequestService.postData<ConversationTree>(url, queryParams);
  }

  listConversations() : Observable<any>{
    const url = environment.get_ListOfconversations;
    const queryParams = { id: 0};
    return this.httpRequestService.getData<any>(url);
  }

  deleteConversation(conversationId: number): Observable<any>{
    const url = environment.delete_conversation;
    console.info('delete conversationId')
    console.info(conversationId)
    let conversationObject = { ConversationId: conversationId};
    const queryParams = { model: conversationObject};
    return this.httpRequestService.postData<any>(url, queryParams);
  }

}
