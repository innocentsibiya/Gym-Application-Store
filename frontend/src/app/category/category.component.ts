import { Component, ViewChild } from '@angular/core';
import { GymItemService } from '../services/gym-item-service.service';
import { CardComponent } from '../card/card.component';
import { CartComponent } from '../cart/cart.component';
import { CartService } from '../services/cart.service';
import { CartItem } from '../interface/CartItem';
import { GymItem } from '../interface/GymItem';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.less']
})
export class CategoryComponent {
  items: GymItem[] = [];
  cartItems: CartItem[] = [];
  @ViewChild(CardComponent) childComponent!: CardComponent;
  @ViewChild(CartComponent) cartComponent!: CartComponent;

  constructor(private gymService: GymItemService,private cartService: CartService) {}

  ngOnInit(): void {
    this.gymService.getAll().subscribe(data => {
      this.items = data;
    });
  }

  showItemDetails(item: GymItem): void {
    this.childComponent.openModal(item);
  }

  addItemToCart(item: CartItem): void {
    this.cartService.addToCart(item).subscribe(cartItems => {
      this.cartItems = cartItems;
    });
  }
}
