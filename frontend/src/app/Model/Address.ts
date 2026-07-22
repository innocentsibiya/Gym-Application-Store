export interface Address {
  id: number;
  userId: number;
  fullName: string;
  street: string;
  city: string;
  province: string;
  country: string;
  postalCode: string;
  addressType: string;
  isDefault: boolean;
}