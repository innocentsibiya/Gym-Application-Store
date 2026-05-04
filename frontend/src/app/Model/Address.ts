export interface Address {
  Id: number;
  UserId: number;
  FullName: string;
  Street: string;
  City: string;
  Province: string;
  Country: string;
  PostalCode: string;
  AddressType:string;
  IsDefault: boolean;
}