import { ConversationInfo } from "../conversationInfo/conversation-info";

export class Conversations {
    list: ConversationInfo[]

    constructor(list: ConversationInfo[]) {
        this.list = list;
    }
}
