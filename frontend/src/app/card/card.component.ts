import { Component, Input } from '@angular/core';
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
    document.body.style.overflow = 'hidden'; 
  }

  closeModal(): void {
    this.isModalOpen = false;
    document.body.style.overflow = 'auto'; 
  }
}