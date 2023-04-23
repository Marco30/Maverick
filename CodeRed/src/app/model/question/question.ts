export class Question {
    conversationId: number;
    text: string;
    mockReply: boolean;
    constructor(conversationId: number, text: string, mockReply: boolean) {
        this.conversationId = conversationId;
        this.text = text;
        this.mockReply = mockReply;
      }
}
