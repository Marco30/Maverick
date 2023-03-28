export class Login {
    socialSecurityNumber: string;
    email: string;
    password: string;
    constructor(socialSecurityNumber: string, email: string, password: string) {
        this.socialSecurityNumber = socialSecurityNumber;
        this.email = email;
        this.password = password;
      }
}
