import { Component } from '@angular/core';
import { GymItem } from '../services/gym-item-service.service';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.less']
})
export class CartComponent {
cartItems: GymItem[] = [];

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.loadCart();
  }

  // Load cart from the API
  loadCart(): void {
      this.cartService.getCart().subscribe(cartItems => {
      this.cartItems = cartItems;
    });
  }

  // Add item to the cart
  addToCart(item: GymItem): void {
    this.cartService.addToCart(item).subscribe(cartItems => {
      this.cartItems = cartItems;
    });
  }

  // Remove item from the cart
  removeFromCart(id: number): void {
    this.cartService.removeFromCart(id).subscribe(cartItems => {
      this.cartItems = cartItems;
    });
  }

  // Calculate the total price of items in the cart
  getTotalPrice(): number {
    return this.cartItems.reduce((total, item) => total + item.price * item.quantity, 0);
  }
}
