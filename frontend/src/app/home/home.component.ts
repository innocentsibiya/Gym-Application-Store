import { Component, ViewChild } from '@angular/core';
import { CardComponent } from '../card/card.component';
import { CartComponent } from '../cart/cart.component';
import { CartService } from '../services/cart.service';
import { ProductService } from '../services/product.service';
import { Product } from '../interface/Product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less']
})
export class HomeComponent {
  items: Product[] = [];
  cartItems: Product[] = [];
  @ViewChild(CardComponent) childComponent!: CardComponent;
  @ViewChild(CartComponent) cartComponent!: CartComponent;

  constructor(private productService: ProductService, private cartService: CartService) {}

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe(data => {
      this.items = data;
    });
  }

  showItemDetails(item: Product): void {
    this.childComponent.openModal(item);
  }

  addItemToCart(item: Product): void {
    this.cartService.addToCart(item).subscribe(cartItems => {
      this.cartItems = cartItems;
    });
  }
}
