export interface CheckoutDetails {
  fullName: string;
  email: string;
  phone: string;
  address: string;
  city: string;
  postalCode: string;

  paymentMethod: 'card' | 'eft';

  cardNumber?: string;
  cardHolder?: string;
  expiryDate?: string;
  cvv?: string;

  bankName?: string;
  accountNumber?: string;
}