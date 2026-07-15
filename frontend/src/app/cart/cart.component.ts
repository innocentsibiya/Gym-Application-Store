import { Component } from '@angular/core';
import { CartService } from '../services/cart.service';
import { CartDto } from '../Model/CartDto';
import { CartItemDto } from '../Model/CartItemDto';


@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.less']
})
export class CartComponent {
  cartItems: CartItemDto[] = [];

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
      this.cartService.loadCart().subscribe((cartItem: CartDto) => {
      this.cartItems = cartItem.items;

    });
  }

  getTotalPrice(): number {
    return this.cartItems.reduce(
      (total, item) => total + item.price * item.quantity,
      0
    );
  }

  trackById(index: number, item: CartItemDto): number {
    return item.productId;
  }

  removeFromCart(id: number): void {
    this.cartService.removeFromCart(id).subscribe(() => {
      this.loadCart();
    });
  }

  increaseQuantity(item: CartItemDto): void {
    item.quantity++;
  }

  decreaseQuantity(item: CartItemDto): void {
    if (item.quantity > 1) {
      item.quantity--;
    } else {
      this.removeFromCart(item.productId);
    }
  }

  proceedToCart(pCartItems: CartItemDto[]) : void{
    // Implement checkout logic here
  }
}
