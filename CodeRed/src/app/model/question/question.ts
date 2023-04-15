export class Question {
    conversationId: number;
    text: string;
    constructor(conversationId: number, text: string) {
        this.conversationId = conversationId;
        this.text = text;
      }
}
