import { User } from '../user/user';

export class AuthToken {
  public token: string;
  public user: User;

  constructor(token: string, user: User) {
    this.token = token;
    this.user = user;
  }
}
