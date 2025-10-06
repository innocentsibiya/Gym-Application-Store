import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../interface/Product';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.less']
})
export class CardComponent {  
  @Input() item!: Product;
  isModalOpen: boolean = false;  

  openModal(item: Product): void {
    this.item = item;
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }
}
