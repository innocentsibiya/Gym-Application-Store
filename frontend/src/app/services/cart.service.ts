import { Injectable } from '@angular/core';
import { Observable, of, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { CartItem } from '../interface/CartItem';

@Injectable({
  providedIn: 'root'
})

export class CartService {
private apiUrl = 'http://localhost:5074/api/Cart';  
private cacheCart: CartItem[] | null = null;
private cartCacheTime = 0;
private cacheTLL = 900000;

  constructor(private http: HttpClient) { }

  // Get cart items from the API
  getCart(forceRefresh : boolean = false): Observable<CartItem[]> {
    if(!forceRefresh && this.cacheCart && (Date.now() - this.cartCacheTime) < this.cacheTLL) {
      return of(this.cacheCart);
    }

    return this.http.get<CartItem[]>(this.apiUrl).pipe(
      tap(items => {
      this.cacheCart = items;
      this.cartCacheTime = Date.now();
    }));
  }

  // Add an item to the cart
  addToCart(item: CartItem): Observable<CartItem[]> {
    var newItem: CartItem = { ...item }; 
    if(this.checkCartItem(item.id)){
      newItem = this.cacheCart!.find(item => item.id == newItem.id)!;
      newItem.quantity += 1;
    }else{
      newItem.quantity = 1;
    }
    return this.http.post<CartItem[]>(this.apiUrl, newItem).pipe(
      tap(items => {
        this.cacheCart = items;
        this.cartCacheTime = Date.now();
      }));
  }

  // Remove an item from the cart
  removeFromCart(id: number): Observable<CartItem[]> {
    return this.http.delete<CartItem[]>(`${this.apiUrl}/${id}`).pipe(
      tap(items => {
        this.cacheCart = items;
        this.cartCacheTime = Date.now();
      }));
  }

  //check if item already exist in a cart
  checkCartItem(id: number): boolean {
    if (this.cacheCart?.some(item => item.id == id)) {
      return true;
    }
    return false;
  }
}
