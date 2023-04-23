export class ConversationInfo {
    public id: number;
    public name : string;
    public date: Date;
    public userId: number;
    constructor(
        id: number,
        name : string,
        date: Date,
        userId: number,
    ) {
      this.id = id;
      this.name = name ;
      this.date = date;
      this.userId = userId;
    }
}
