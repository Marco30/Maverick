import { Answer } from "../answer/answer";
import { Question } from "../question/question";

export class Conversation {
    public question: Question;
    public answers : Answer[];

    constructor(
        question: Question,
        answers :  Answer[],
    ) {
      this.question = question;
      this.answers = answers ; 
    }
}
