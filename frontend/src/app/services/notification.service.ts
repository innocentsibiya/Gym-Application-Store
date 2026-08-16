import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface ToastMessage {
  type: 'info' | 'success' | 'warning' | 'error';
  text: string;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private toastSubject = new Subject<ToastMessage>();
  toast$ = this.toastSubject.asObservable();

  show(type: ToastMessage['type'], text: string) {
    console.log('Emitting toast:', type, text); // DEBUG
    this.toastSubject.next({ type, text });
  }
}
