import { Component, ElementRef, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-ai-chat',
  templateUrl: './ai-chat.component.html',
  styleUrls: ['./ai-chat.component.css'],
})
export class AiChatComponent {
  messages: any[] = [];
  newMessage: string = '';
  @ViewChild('chatContainer', { static: false }) private chatContainer: ElementRef | null = null;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    console.info('Marco test');

    this.messages = [
      { sender: 'Ava', content: 'Hi there! How can I help you?' },
      { sender: 'User', content: 'Can you tell me more about your product?' },
      { sender: 'Ava', content: 'Sure! Our product is designed to...' },
      { sender: 'User', content: 'That sounds great. How do I get started?' },
      { sender: 'Ava', content: 'You can get started by...' },
      { sender: 'Ava', content: 'Hi there! How can I help you?' },
      { sender: 'User', content: 'Can you tell me more about your product?' },
      { sender: 'Ava', content: 'Sure! Our product is designed to...' },
      { sender: 'User', content: 'That sounds great. How do I get started?' },
      { sender: 'Ava', content: 'You can get started by...' },
      { sender: 'Ava', content: 'Hi there! How can I help you?' },
      { sender: 'User', content: 'Can you tell me more about your product?' },
      { sender: 'Ava', content: 'Sure! Our product is designed to...' },
      { sender: 'User', content: 'That sounds great. How do I get started?' },
      { sender: 'Ava', content: 'You can get started by...' },
    ];

    this.exampleAnswers();
  }

  ngAfterViewInit() {
    if (this.chatContainer) {
      this.scrollToLastMessage()
    }
  }

  scrollToLastMessage(): void {
    
    setTimeout(() => {

    const element = document.getElementById("advanced-tools"); // Your target element
    const headerOffset = 45;

    if (element) {
      const elementPosition = element.getBoundingClientRect().top;
      const offsetPosition = elementPosition + window.pageYOffset - headerOffset;


      window.scrollTo({
        top: offsetPosition,
        behavior: "smooth",
      });
    }
    /* const lastMessageIndex = this.messages.length - 1;
    const lastMessageId = 'message-' + lastMessageIndex;
    const lastMessageElement = document.getElementById('advanced-tools');
    if (lastMessageElement) {
      lastMessageElement.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    } */
  },300);

  }



  exampleAnswers() {
    this.messages.push({
      sender: 'Ava',
      content: 'Hello! How can I help you today?',
    });
    this.messages.push({
      sender: 'User',
      content: 'Can you tell me about the weather in New York?',
    });
    this.messages.push({
      sender: 'Ava',
      content:
        'Sure! The weather in New York is currently 70 degrees and sunny.',
    });
    this.messages.push({
      sender: 'User',
      content: 'What is the capital of France?',
    });
    this.messages.push({
      sender: 'Ava',
      content: 'The capital of France is Paris.',
    });
    this.messages.push({ sender: 'User', content: 'Thanks for your help!' });
    this.messages.push({
      sender: 'Ava',
      content: 'Youre welcome. Have a great day!',
    });
  }

  onSubmit() {
    /*if (this.newMessage) {
      this.messages.push({ sender: 'User', content: this.newMessage });
      this.http
        .post(
          'https://api.openai.com/v1/engines/davinci-codex/completions',
          {
            prompt: this.newMessage,
            max_tokens: 150,
            n: 1,
            stop: '\n',
            temperature: 0.7,
          },
          {
            headers: {
              Authorization: 'Bearer ' + 'YOUR_API_KEY_HERE',
              'Content-Type': 'application/json',
            },
          }
        )
        .subscribe((response: any) => {
          const botMessage = response.choices[0].text.trim();
          this.messages.push({ sender: 'Bot', content: botMessage });
        });
      this.newMessage = '';
    } */

    this.messages.push({
      sender: 'Ava',
      content: 'Youre welcome. Have a great day!',
    });

 

      this.scrollToLastMessage();


  }

}
