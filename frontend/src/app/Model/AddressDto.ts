export interface AddressDto {
  id: number;
  userId: number;
  street: string;
  city: string;
  province: string;
  postalCode: string;
  country: string;
  addressType: string;
  isDefault: boolean;
}