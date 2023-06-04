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

  private conversationsList$: BehaviorSubject<ConversationInfo[]> = new BehaviorSubject<ConversationInfo[]>([]);
  selectedConversationsList$: Observable<ConversationInfo[]> = this.conversationsList$.asObservable();

  
  private savedConversationsList$: BehaviorSubject<ConversationInfo[]> = new BehaviorSubject<ConversationInfo[]>([]);
  selectedSavedConversationsList$: Observable<ConversationInfo[]> = this.savedConversationsList$.asObservable();

  conversations = new Conversations([]);

  constructor() { }

  public setConversationsInfo(conversationsInfo: ConversationInfo) {
    this.conversationInfo$.next(conversationsInfo);
  }

  public getConversationsInfo() {
    return this.conversationInfo$.value;
  }

  public setConversationsList(conversationsList: ConversationInfo[]) {
    this.conversationsList$.next(conversationsList);
  }

  public getConversationsList() {
    return this.conversationsList$.value;
  }

  public setSavedConversationsList(conversationsList: ConversationInfo[]) {
    this.savedConversationsList$.next(conversationsList);
  }

  public getSavedConversationsList() {
    return this.savedConversationsList$.value;
  }


}
