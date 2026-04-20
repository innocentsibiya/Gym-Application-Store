import { Product } from "../interface/Product";
import { User } from "../interface/User";
import { CartItem } from "./CartItem";

export interface Cart {
  id: number;
  UserId: number;
  CreatedAt: Date;
  UpdatedAt?: Date;
  User: User;
  items: CartItem[];
}