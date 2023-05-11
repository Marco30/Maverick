import { Component, ElementRef, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Question } from 'src/app/model/question/question';
import { ConversationService } from 'src/app/service/conversation/conversation.service';
import { SharedDataService } from 'src/app/service/sharedData/shared-data.service';
import { ConversationInfo } from 'src/app/model/conversationInfo/conversation-info';
import { Subject, takeUntil } from 'rxjs';
import { Conversation } from 'src/app/model/conversation/conversation';
import { ConversationTree } from 'src/app/model/conversationTree/conversation-tree';
import { Answer } from 'src/app/model/answer/answer';
import { AuthenticationService } from 'src/app/service/authentication/authentication.service';


@Component({
  selector: 'app-ai-chat',
  templateUrl: './ai-chat.component.html',
  styleUrls: ['./ai-chat.component.css'],
})
export class AiChatComponent {

  constructor(private conversationService: ConversationService,private authenticationService: AuthenticationService, private sharedDataService: SharedDataService) { }

  onDestroy$: Subject<boolean> = new Subject();

  messages: any[] = [];
  newMessage: string = '';
  @ViewChild('chatContainer', { static: false }) private chatContainer: ElementRef | null = null;
  showError = false;
  errorMessage = '';
  questionData: Question = {
    conversationId: 0,
    text: '',
    mockReply: true,
  };
  newConversation = true;
  AIname = 'Ava';
  userName = this.authenticationService.getDataFromToken('firstname');

  answer1 = new Answer(1, 'Answer 1', new Date(), 1, 1);
  answer2 = new Answer(2, 'Answer 2', new Date(), 1, 1);
  answers = [this.answer1, this.answer2];

  question = new Question(1, 'Question 1', false);

  conversation = new Conversation(this.question, this.answers);

  conversationTree: ConversationTree = new ConversationTree([this.conversation]);
  
  conversationInfo = new ConversationInfo(0 , '', new Date, 0 );

  
  ngOnInit() {
    this.getConversations()
    this.getConversationInfo()
    console.info('Marco test');
    console.info(this.conversationInfo);

  
  }

  ngOnDestroy(): void {

    this.onDestroy$.next(true);
    this.onDestroy$.unsubscribe();
  
}


  getConversationData(){


    this.conversationService.conversation(this.conversationInfo.id).pipe(takeUntil(this.onDestroy$)).subscribe({
      next: (res) => {

        console.info('-----AIFulConversation-----');
        this.conversationTree.conversation = res.conversation;
        this.scrollToLastMessage();
        console.info(this.conversationTree.conversation);
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

  getConversationInfo(){

    this.sharedDataService.selectedConversationInfo$.pipe(takeUntil(this.onDestroy$)).subscribe((value) => {
      
      this.conversationInfo = value;
      if(this.conversationInfo.id != 0){
        this.newConversation = false;
      this.getConversationData();
      }else{
        this.newConversation = true;
        this.resetConversationToEmpty();
      }

    });
  }

  resetConversationToEmpty(){
    const answerTemp1 = new Answer(1, 'Answer 1', new Date(), 1, 1);
    const answerTemp2= new Answer(2, 'Answer 2', new Date(), 1, 1);
    const answersTemp = [answerTemp1, answerTemp2];
  
    const questionTemp = new Question(1, 'Question 1', false);
  
    const conversationTemp = new Conversation(questionTemp, answersTemp);

    const conversationTreeTemp: ConversationTree = new ConversationTree([conversationTemp]);

    this.conversationTree = conversationTreeTemp;

  }

  ngAfterViewInit() {
    if (this.chatContainer) {
      this.scrollToLastMessage()
    }
  }

  scrollToLastMessage(): void {
    
    const element = document.getElementById("advanced-tools"); // Your target element
    const headerOffset = 45;
    const isScrolledToTarget = element && element.getBoundingClientRect().top <= headerOffset;
  
    if (!isScrolledToTarget) {
      setTimeout(() => {
        if (element) {
          const elementPosition = element.getBoundingClientRect().top;
          const offsetPosition = elementPosition + window.pageYOffset - headerOffset;
  
          window.scrollTo({
            top: offsetPosition,
            behavior: "smooth",
          });
          /*    const lastMessageIndex = this.messages.length - 1;
    const lastMessageId = 'message-' + lastMessageIndex;
    const lastMessageElement = document.getElementById('advanced-tools');
    if (lastMessageElement) {
      lastMessageElement.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }  */

        }
      }, 300);
    }

   
  }



  getConversations(){


    this.conversationService.listConversations().pipe(takeUntil(this.onDestroy$)).subscribe({
      next: (res) => {
        console.info('-----NewAIlistConversation-----');
        console.info(res);
        this.sharedDataService.setConversationsList(res);
        
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

 

  
  onSubmit() {

    if (this.questionData.text != '') {

      if(this.conversationInfo.id != 0){
      this.questionData.conversationId = this.conversationInfo.id;
      }

      this.questionData.mockReply = true;
     

      this.conversationService.askTheAI(this.questionData).pipe(takeUntil(this.onDestroy$)).subscribe({
        next: (res) => {

          console.info('-----AskTheAI-----');
          console.info(res);
          let answerData = new Answer(res.id, '', res.date, res.questionId, res.conversationId);
          let conversationData = new Conversation(this.questionData, [answerData]);
          this.conversationTree.conversation.push(conversationData); 
          if(res.text){
          this.revealText(res.text,10);
          }
          this.getConversations();

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

   
   // var testtext = 'I am ChatGPT, a large language model developed by OpenAI using the GPT-3.5 architecture. As a language model, my primary function is to generate natural language responses to text-based inputs such as questions, prompts, and statements.\r\n\r\nI was trained on a massive dataset of written text from the internet and other sources, which allows me to understand and generate responses in a wide variety of topics and domains. My training data includes text in multiple languages, making me capable of generating responses in several languages.\r\n\r\nI use machine learning techniques such as deep neural networks to analyze and understand the structure and context of the input text. Based on this analysis, I generate a response that is meant to be natural-sounding and relevant to the input.\r\n\r\nOverall, my purpose is to assist users in generating high-quality, natural language responses to a wide range of prompts and queries, making communication and information retrieval more efficient and effective.';
   // this.revealText(testtext,10);


    console.info("questionData");
 console.info(this.questionData);

      this.scrollToLastMessage();


  }


  revealText(text: string, delay: number) {
    let index = 0;
    let interval = setInterval(() => {
      //console.log(text.charAt(index));
      this.conversationTree.conversation[this.conversationTree.conversation.length - 1].answers[0].text += text.charAt(index);
      this.scrollToLastMessage();
      index++;
      if (index >= text.length) {
        clearInterval(interval);
      }
    }, delay);
  }



}
