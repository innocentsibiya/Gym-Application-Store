import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { GymItem, GymItemService } from './services/gym-item-service.service';
import { CardComponent } from './card/card.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'frontend';
  items: GymItem[] = [];
@ViewChild(CardComponent) childComponent!: CardComponent;

  constructor(private gymService: GymItemService) {}

  ngOnInit(): void {
    this.gymService.getAll().subscribe(data => {
      this.items = data;
    });
  }

  showItemDetails(item: GymItem): void {
    this.childComponent.openModal(item);
  }

}
