import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ConversationInfo } from 'src/app/model/conversationInfo/conversation-info';


@Injectable({
  providedIn: 'root'
})
export class SharedDataService {
  private conversationInfo$: BehaviorSubject<ConversationInfo> = new BehaviorSubject<ConversationInfo>({ id: 0, name : '', date: new Date, userId: 0, });
  selectedConversationInfo$: Observable<ConversationInfo> = this.conversationInfo$.asObservable();

  constructor() { }

  public setConversationsInfo(conversationsInfo: ConversationInfo) {
    this.conversationInfo$.next(conversationsInfo);
  }

  public getConversationsInfo() {
    return this.conversationInfo$.value;
  }

}
