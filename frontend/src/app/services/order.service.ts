import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Order } from '../Model/Order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = 'http://localhost:5074/api/Orders';

  private orderCountSubject = new BehaviorSubject<number>(0);
  orderCount$ = this.orderCountSubject.asObservable();

  constructor(private http: HttpClient) {}

  placeOrder(order: Partial<Order>): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/place`, order).pipe(
      tap(() => this.refreshOrderCount(order.userId!))
    );
  }

  getOrdersByUserId(userId: number = 1): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/user/${userId}`).pipe(
      tap(orders => this.orderCountSubject.next(orders.length))
    );
  }
  private refreshOrderCount(userId: number): void {
    this.getOrdersByUserId(userId).subscribe();
  }

  getOrderCount(): number {
    return this.orderCountSubject.value;
  }

  getOrdersByYear(userId: number = 1, year: number) {
    return this.http.get(`${this.apiUrl}/user/${userId}/year/${year}`);
  }
}