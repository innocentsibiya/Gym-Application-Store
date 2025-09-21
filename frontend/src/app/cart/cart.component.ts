import { Component } from '@angular/core';
import { CartService } from '../services/cart.service';
import { CartItem } from '../interface/CartItem';


@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.less']
})
export class CartComponent {
  cartItems: CartItem[] = [];

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
      this.cartService.getCart().subscribe(cartItems => {
      this.cartItems = cartItems;
    });
  }

  getTotalPrice(): number {
    return this.cartItems.reduce(
      (total, item) => total + item.price * item.quantity,
      0
    );
  }

  trackById(index: number, item: CartItem): number {
    return item.id;
  }

  removeFromCart(id: number): void {
    this.cartService.removeFromCart(id).subscribe(items =>{
      this.cartItems = items;
    });
  }

  increaseQuantity(item: CartItem): void {
    item.quantity++;
  }

  decreaseQuantity(item: CartItem): void {
    if (item.quantity > 1) {
      item.quantity--;
    } else {
      this.removeFromCart(item.id);
    }
  }
}
