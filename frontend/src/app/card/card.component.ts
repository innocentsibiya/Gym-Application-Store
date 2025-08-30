import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GymItem, GymItemService } from '../services/gym-item-service.service';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.less']
})
export class CardComponent {
  @Input() item!: GymItem;  // Accept gym item data from parent
  isModalOpen: boolean = false;  // Flag to manage modal visibility

  // Method to open the modal
  openModal(item: GymItem): void {
    this.item = item;
    this.isModalOpen = true;
  }

  // Method to close the modal
  closeModal(): void {
    this.isModalOpen = false;
  }
}
