import { Address } from '../address/address';

export const GENDERS = {
  male: 'Male',
  female: 'Female',
  other: 'Other',
} as const;

type Object<T> = T[keyof T];
export type Genders = Object<typeof GENDERS>;
export class Register {
  firstName: string;
  lastName: string;
  fullName: string;
  socialSecurityNumber: string;
  email: string;
  password: string;
  agreeMarketing: boolean;
  subscribeToEmailNotification: boolean;
  gender: Genders | null;

  address: Address;
  phoneNumber: number | null;
  mobilePhoneNumber: number | null;
  birthDate: string | null;
  constructor(
    socialSecurityNumber: string,
    email: string,
    password: string,
    firstName: string,
    lastName: string,
    agreeMarketing: boolean,
    gender: Genders,
    fullName: string,
    subscribeToEmailNotification: boolean,

    address: Address,
    phoneNumber: number,
    mobilePhoneNumber: number,
    birthDate: string
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

    this.address = address;
    this.phoneNumber = phoneNumber;
    this.mobilePhoneNumber = mobilePhoneNumber;
    this.birthDate = birthDate;
  }
}
