import { OrderItem } from "./OrderItem";

export interface Order {
  id: number;
  userId: number;
  orderStatus: string;
  paymentStatus: string;
  shippingAddressId: number;
  billingAddressId: number;
  subtotal: number;
  tax: number;
  shippingCost: number;
  discount?: number;
  totalAmount: number;
  createdAt: string;
  updatedAt?: string;

  items?: OrderItem[];
}