import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Product } from '../interface/Product';
import { CartItem } from '../Model/CartItem';
import { Cart } from '../Model/Cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'http://localhost:5074/api/Cart';
  private defaultUserId = 1;

  private cacheCart: Product[] = [];
  private cartCacheTime = 0;
  private cacheTTL = 900000; // 15 minutes

  private cartSubject = new BehaviorSubject<Product[]>([]);
  public cart$ = this.cartSubject.asObservable();

  constructor(private http: HttpClient) {}

  /** Get cart, optionally force refresh */
  getCart(forceRefresh: boolean = false): Observable<Product[]> {
    if (!forceRefresh && this.cacheCart.length && (Date.now() - this.cartCacheTime) < this.cacheTTL) {
      // return of(this.cacheCart);
    }

    const url = `${this.apiUrl}/${this.defaultUserId}`;
    return this.http.get<Cart>(url).pipe(
        map(cart => {
          // extract products from cart items
          return cart.items
            .filter(item => item.quantity > 0) // optional cleanup
            .map(item => item.product);
        }),
        tap(products => {
          // update cache
          this.cacheCart = products;
          this.cartCacheTime = Date.now();
        }),
        catchError(err => {
          console.error('Failed to load cart:', err);
          return of(this.cacheCart); // fallback
        })
      );
  }

  /** Add a product to the cart */
  addToCart(item: Product): Observable<Product[]> {
    const currentCart = this.cartSubject.value || [];
    let newItem: Product = { ...item };

    const existingItem = currentCart.find(i => i.id === newItem.id);
    if (existingItem) {
      existingItem.stockQuantity += 1;
      newItem = existingItem;
    } else {
      newItem.stockQuantity = 1;
    }

    const url = `${this.apiUrl}/${this.defaultUserId}/add/${newItem.id}`;
    const params = new HttpParams().set('quantity', newItem.stockQuantity.toString());

    const tempCart = this.http.post<Cart>(url, null, { params }).pipe(
    map(cart => {
      if (!cart || !cart.items) {
        console.warn('Cart missing items:', cart);
        return [];
      }
      
      return cart.items
        .map(i => i.product);
    }),
      tap(products => {
        this.cacheCart = products;
        this.cartCacheTime = Date.now();
        this.cartSubject.next(products);
      }),
      catchError(err => {
        console.error('Failed to add to cart:', err);
        return of(this.cartSubject.value || []);
      })
    );

    return tempCart;
  }

  /** Remove a product from the cart */
  removeFromCart(productId: number): Observable<Product[]> {
    const url = `${this.apiUrl}/${this.defaultUserId}/remove/${productId}`;
    return this.http.delete<Product[]>(url).pipe(
      tap(items => this.updateCart(items)),
      catchError(err => {
        console.error('Failed to remove from cart:', err);
        return of(this.cartSubject.value);
      })
    );
  }

  /** Check if product exists in cart */
  checkCartItem(productId: number): boolean {
    return this.cartSubject.value?.some(item => item.id === productId) ?? false;
  }

  /** Update BehaviorSubject and cache */
  private updateCart(items: Product[]) {
    const safeItems = items || [];
    this.cartSubject.next(safeItems);
    this.cacheCart = safeItems;
    this.cartCacheTime = Date.now();
  }

  /** Clear cart cache manually */
  clearCache() {
    this.cacheCart = [];
    this.cartCacheTime = 0;
    this.cartSubject.next([]);
  }
}