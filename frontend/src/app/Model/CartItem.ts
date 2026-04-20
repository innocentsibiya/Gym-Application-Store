import { Product } from "../interface/Product";

export interface CartItem {
  id: number;
  cartId: number;
  productId: number;
  quantity: number;
  price: number;
  product: Product
}