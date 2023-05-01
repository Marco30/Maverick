import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ConversationInfo } from 'src/app/model/conversationInfo/conversation-info';
import { Conversations } from 'src/app/model/conversations/conversations';


@Injectable({
  providedIn: 'root'
})
export class SharedDataService {
  private conversationInfo$: BehaviorSubject<ConversationInfo> = new BehaviorSubject<ConversationInfo>({ id: 0, name : '', date: new Date, userId: 0, });
  selectedConversationInfo$: Observable<ConversationInfo> = this.conversationInfo$.asObservable();

  private ConversationsList$: BehaviorSubject<ConversationInfo[]> = new BehaviorSubject<ConversationInfo[]>([]);
  selectedConversationsList$: Observable<ConversationInfo[]> = this.ConversationsList$.asObservable();

  conversations = new Conversations([]);

  constructor() { }

  public setConversationsInfo(conversationsInfo: ConversationInfo) {
    this.conversationInfo$.next(conversationsInfo);
  }

  public getConversationsInfo() {
    return this.conversationInfo$.value;
  }

  public setConversationsList(conversationsList: ConversationInfo[]) {
    this.ConversationsList$.next(conversationsList);
  }

  public getConversationsList() {
    return this.ConversationsList$.value;
  }

}
