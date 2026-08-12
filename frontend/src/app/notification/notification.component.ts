import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.less']
})
export class NotificationComponent {
  @Input() title: string = 'Notification';
  @Input() message: string = 'Action completed successfully!';
  @Input() details?: { label: string; value: string }[];

  constructor(private router: Router) {}

  goHome(): void {
    this.router.navigate(['/']);
  }
}