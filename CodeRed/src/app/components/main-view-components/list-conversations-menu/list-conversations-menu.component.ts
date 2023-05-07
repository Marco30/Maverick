import { Component } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { ConversationInfo } from 'src/app/model/conversationInfo/conversation-info';
import { Conversations } from 'src/app/model/conversations/conversations';
import { ConversationService } from 'src/app/service/conversation/conversation.service';
import { SharedDataService } from 'src/app/service/sharedData/shared-data.service';

@Component({
  selector: 'app-list-conversations-menu',
  templateUrl: './list-conversations-menu.component.html',
  styleUrls: ['./list-conversations-menu.component.css']
})
export class ListConversationsMenuComponent {

  constructor(private conversationService: ConversationService, private sharedDataService: SharedDataService) { }

  showError = false;
  errorMessage = '';
 
  
  conversations = new Conversations([]);

  public isMoveable = false;

  onDestroy$: Subject<boolean> = new Subject();

  ngOnInit(): void {

    this.getConversations();
    const itemOrder = sessionStorage.getItem('ListConversationsOrder');
    if (itemOrder) {
      const orderedItems = JSON.parse(itemOrder).map((index: number) => this.conversations.list[index]);
      this.conversations = orderedItems;
    }
  }


  getConversations(){

   
    this.sharedDataService.selectedConversationsList$.pipe(takeUntil(this.onDestroy$)).subscribe((value) => {
      
    if(value){
      this.conversations.list = value;
      console.info('-----AIlistConversation-----');
      console.info(this.conversations.list);
    }
    

    });

    /*this.conversationService.listConversations().pipe(takeUntil(this.onDestroy$)).subscribe({
      next: (res) => {

       this.conversations.list = res;
        console.info('-----AIlistConversation-----');
        console.info(this.conversations.list);
        
      },
      error: (err) => {
        console.error('An error occurred:', err);
        this.errorMessage = 'An error occurred while processing your request. Please try again later.';
        this.showError = true;   
        setTimeout(() => {
          this.showError = false;
        }, 5000); // hide after 5 seconds
      },
      // complete: () => (this.showLoading = false),
    });*/
  
  }

  ngOnDestroy(): void {

      this.onDestroy$.next(true);
      this.onDestroy$.unsubscribe();
    
  }

  onNameChange(event: Event, conversation: ConversationInfo): void {
    const newName = (event.target as HTMLTableCellElement).innerText.trim();
    if (newName !== conversation.name) {
      conversation.name = newName;

      // TODO: call your service to save the updated conversation name

      this.conversationService.changeConversationName(conversation).pipe(takeUntil(this.onDestroy$)).subscribe({
        next: (res) => {
  
          console.info('-----AIConversatioConversationName-----');
          console.info(res);
        },
        error: (err) => {
          console.error('An error occurred:', err);
          this.errorMessage = 'An error occurred while processing your request. Please try again later.';
          this.showError = true;   
          setTimeout(() => {
            this.showError = false;
          }, 5000); // hide after 5 seconds
        },
        // complete: () => (this.showLoading = false),
      });

    }
  }


  

  newConversation(){
    const conversationInfo = new ConversationInfo(0 , '', new Date, 0 );
    this.sharedDataService.setConversationsInfo(conversationInfo);
  }

  openConversation(item: any){
    console.info('testar att man kan klicka här'+ item.id);
    this.sharedDataService.setConversationsInfo(item);
  }

  public toggleMoveable(): void {
    this.isMoveable = !this.isMoveable;
  }

  public onDragStart(event: DragEvent, index: number): void {
    if (event.dataTransfer) {
    event.dataTransfer.setData('text/plain', index.toString());
  }
  }

  public onDragOver(event: DragEvent): void {
    event.preventDefault();
  }

  public onDrop(event: DragEvent, index: number): void {
    event.preventDefault();
    if (event.dataTransfer) {
    const oldIndex = parseInt(event.dataTransfer.getData('text/plain'), 10);
    const element = this.conversations.list[oldIndex];
    this.conversations.list.splice(oldIndex, 1);
    this.conversations.list.splice(index, 0, element);
    sessionStorage.setItem('ListConversationsOrder', JSON.stringify(this.conversations.list.map((_, i) => i)));
  }
  
  }

  public sortByDateDescending(): void {
    this.conversations.list.sort((a, b) => {
      const dateA = new Date(a.date).getTime();
      const dateB = new Date(b.date).getTime();
      return dateB - dateA;
    });
  }

  public sortByDateAscending(): void {
    this.conversations.list.sort((a, b) => {
      const dateA = new Date(a.date).getTime();
      const dateB = new Date(b.date).getTime();
      return dateA - dateB;
    });
  }

  public onDateSortChange(event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    if (checkbox.checked) {
      this.sortByDateDescending();
    } else {
      this.sortByDateAscending();
    }
  }

}
