export class Answer {
    public id: number;
    public text : string;
    public date: Date;
    public questionId: number;
    public conversationId : number;
    constructor(
        id: number,
        text : string,
        date: Date,
        questionId: number,
        conversationId : number,

    ) {
      this.id = id;
      this.text = text ;
      this.date = date;
      this.questionId = questionId;
      this.conversationId  = conversationId ;
 
    }
}
