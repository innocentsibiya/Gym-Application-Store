import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Product } from '../interface/Product';
import { CartDto } from '../Model/CartDto';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private apiUrl = 'http://localhost:5074/api/Cart';
  private defaultUserId = 1;

  private cartSubject = new BehaviorSubject<CartDto | null>(null);

  public cart$ = this.cartSubject.asObservable();

  constructor(private http: HttpClient) {}

  loadCart(): Observable<CartDto> {
    const url = `${this.apiUrl}/${this.defaultUserId}`;

    return this.http.get<CartDto>(url).pipe(
      tap(cart => {
        this.cartSubject.next(cart);
      }),
      catchError(error => {
        console.error('Failed to load cart', error);
        return throwError(() => error);
      })
    );
  }

  getCart(): CartDto | null {
    return this.cartSubject.value;
  }

  addToCart(product: Product): Observable<CartDto> {
    const existingItem = this.cartSubject.value?.items?.find(
      i => i.productId === product.id
    );

    const quantity = existingItem
      ? existingItem.quantity + 1
      : 1;

    const url = `${this.apiUrl}/${this.defaultUserId}/add/${product.id}`;

    const params = new HttpParams()
      .set('quantity', quantity.toString());

    return this.http.post<CartDto>(
      url,
      null,
      { params }
    ).pipe(
      tap(cart => {
        this.cartSubject.next(cart);
      }),
      catchError(error => {
        console.error('Failed to add item', error);
        return throwError(() => error);
      })
    );
  }

  removeFromCart(productId: number): Observable<CartDto> {

    const url = `${this.apiUrl}/${this.defaultUserId}/remove/${productId}`;

    return this.http.delete<CartDto>(url).pipe(
      tap(cart => {
        this.cartSubject.next(cart);
      }),
      catchError(error => {
        console.error('Failed to remove item', error);
        return throwError(() => error);
      })
    );
  }

  checkCartItem(productId: number): boolean {

    return this.cartSubject.value?.items.some(
      item => item.productId === productId
    ) ?? false;
  }

  getCartCount(): number {

    return this.cartSubject.value?.items.reduce(
      (sum, item) => sum + item.quantity,
      0
    ) ?? 0;
  }

  clearCart(): void {
    this.cartSubject.next(null);
  }
}