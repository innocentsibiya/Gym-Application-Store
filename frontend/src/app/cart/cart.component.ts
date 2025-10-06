import { Component } from '@angular/core';
import { CartService } from '../services/cart.service';
import { Product } from '../interface/Product';


@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.less']
})
export class CartComponent {
  cartItems: Product[] = [];

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
      (total, item) => total + item.price * item.stockQuantity,
      0
    );
  }

  trackById(index: number, item: Product): number {
    return item.id;
  }

  removeFromCart(id: number): void {
    this.cartService.removeFromCart(id).subscribe(items =>{
      this.cartItems = items;
    });
  }

  increaseQuantity(item: Product): void {
    item.stockQuantity++;
  }

  decreaseQuantity(item: Product): void {
    if (item.stockQuantity > 1) {
      item.stockQuantity--;
    } else {
      this.removeFromCart(item.id);
    }
  }
}
