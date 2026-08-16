import { Component, OnInit } from '@angular/core';
import { NotificationService, ToastMessage } from '../services/notification.service';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.less']
})
export class ToastComponent implements OnInit {
  messages: { msg: ToastMessage; progress: number }[] = [];

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.notificationService.toast$.subscribe(msg => {
      const toast = { msg, progress: 100 };
      this.messages.push(toast);

      const interval = setInterval(() => {
        toast.progress -= 5;
        if (toast.progress <= 0) {
          clearInterval(interval);
          this.removeToast(toast);
        }
      }, 100);
    });
  }

  removeToast(toast: { msg: ToastMessage; progress: number }) {
    const index = this.messages.indexOf(toast);
    if (index > -1) this.messages.splice(index, 1);
  }
}