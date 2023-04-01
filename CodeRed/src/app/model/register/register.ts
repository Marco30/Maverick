import { Address } from "../address/address";

export class Register {
  firstName: string;
  lastName: string;
  fullName: string;
  socialSecurityNumber: string;
  email: string;
  password: string;
  agreeMarketing: boolean;
  subscribeToEmailNotification: boolean;
  gender: string;
  birthYear: number;
  birthMonth: number;
  birthDay: number;
  address: Address;
  constructor(
    socialSecurityNumber: string,
    email: string,
    password: string,
    firstName: string,
    lastName: string,
    agreeMarketing: boolean,
    gender: string,
    fullName: string,
    subscribeToEmailNotification: boolean,
    birthDay: number,
    birthYear: number,
    birthMonth: number,
    address: Address 
  ) {
    this.socialSecurityNumber = socialSecurityNumber;
    this.email = email;
    this.password = password;
    this.firstName = firstName;
    this.lastName = lastName;
    this.agreeMarketing = agreeMarketing;
    this.subscribeToEmailNotification = subscribeToEmailNotification;
    this.gender = gender;
    this.fullName = fullName;
    this.birthDay = birthDay;
    this.birthMonth = birthMonth;
    this.birthYear = birthYear;
    this.address = address; 
  }
}
