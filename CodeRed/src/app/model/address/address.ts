export class Address {
  public city: string;
  public country: string;
  public street: string;
  public zipCode: string;
  public attention: string;
  public careOf: string;
  public municipality: string;
  constructor(
    city: string,
    country: string,
    street: string,
    zipCode: string,
    attention: string,
    careOf: string,
    municipality: string
  ) {
    this.city = city;
    this.country = country;
    this.street = street;
    this.zipCode = zipCode;
    this.attention = attention;
    this.careOf = careOf;
    this.municipality = municipality;
  }
}
