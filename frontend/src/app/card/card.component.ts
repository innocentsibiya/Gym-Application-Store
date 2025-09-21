import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GymItemService } from '../services/gym-item-service.service';
import { GymItem } from '../interface/GymItem';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.less']
})
export class CardComponent {
  @Input() item!: GymItem;  
  isModalOpen: boolean = false;  

  openModal(item: GymItem): void {
    this.item = item;
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }
}
