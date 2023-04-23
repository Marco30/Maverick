import { Conversation } from "../conversation/conversation";

export class ConversationTree {
    public conversation : Conversation[];
    constructor(
        conversation : Conversation[]
    ) {
      this.conversation = conversation; 
    }
}
