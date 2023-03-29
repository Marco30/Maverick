import { Address } from "../address/address";


export class User {
  public id: number;
  public socialSecurityNumber: string;
  public firstName: string;
  public lastName: string;
  public fullName: string;
  public email: string;
  public password: string;
  public phoneNumber: string;
  public mobilePhoneNumber: string;
  public agreeMarketing: boolean;
  public subscribeToEmailNotification: boolean;
  public profileImage: string;
  public address: Address;
  public gender: string;

  constructor(id: number, socialSecurityNumber: string, firstName: string, lastName: string, fullName: string, email: string, password: string, phoneNumber: string, mobilePhoneNumber: string, agreeMarketing: boolean, subscribeToEmailNotification: boolean, profileImage: string, address: Address, gender: string) {
    this.id = id;
    this.socialSecurityNumber = socialSecurityNumber;
    this.firstName= firstName;
    this.lastName = lastName;
    this.fullName = fullName;
    this.email= email;
    this.password = password;
    this.phoneNumber = phoneNumber;
    this.mobilePhoneNumber = mobilePhoneNumber;
    this.agreeMarketing = agreeMarketing;
    this.subscribeToEmailNotification = subscribeToEmailNotification;
    this.profileImage= profileImage;
    this.address = address;
    this.gender = gender;
  }
}
