import { Component, ViewChild } from '@angular/core';
import { GymItem, GymItemService } from './services/gym-item-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'frontend';
  items: GymItem[] = [];
  gymItem : GymItem | undefined;

  constructor(private gymService: GymItemService) {}

  ngOnInit(): void {
    this.gymService.getAll().subscribe(data => {
      this.items = data;
    });

    this.gymService.getById(1).subscribe(item => {
      this.gymItem = item;
    });
  }

}
