import { GENDERS, Genders } from 'src/app/model/register/register';

export class UserData {
    public socialSecurityNumber?: string;
    public firstName?: string;
    public lastName?: string;
    public fullName?: string;
    public gender?: Genders;
    public city?: string;
    public street?: string;
    public zipCode?: string;
    public phoneNumber?: number;
    public mobilePhoneNumber?: number;
  
    constructor(
      socialSecurityNumber?: string,
      firstName?: string,
      lastName?: string,
      fullName?: string,
      gender?: Genders,
      city?: string,
      street?: string,
      zipCode?: string,
      phoneNumber?: number,
      mobilePhoneNumber?: number
    ) {
      this.socialSecurityNumber = socialSecurityNumber;
      this.firstName = firstName;
      this.lastName = lastName;
      this.fullName = fullName;
      this.gender = gender;
      this.city = city;
      this.street = street;
      this.zipCode = zipCode;
      this.phoneNumber = phoneNumber;
      this.mobilePhoneNumber = mobilePhoneNumber;
    }
  }
  