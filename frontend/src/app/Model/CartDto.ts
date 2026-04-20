import { CartItemDto } from "./CartItemDto";

export interface CartDto {
  UserId: number;
  Items: CartItemDto[];
  TotalPrice: number;
}