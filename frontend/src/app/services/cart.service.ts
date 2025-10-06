import { Injectable } from '@angular/core';
import { Observable, of, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Product } from '../interface/Product';

@Injectable({
  providedIn: 'root'
})

export class CartService {
private apiUrl = 'http://localhost:5074/api/Cart';  
private cacheCart: Product[] | null = null;
private cartCacheTime = 0;
private cacheTLL = 900000;

  constructor(private http: HttpClient) { }

  // Get cart items from the API
  getCart(forceRefresh : boolean = false): Observable<Product[]> {
    if(!forceRefresh && this.cacheCart && (Date.now() - this.cartCacheTime) < this.cacheTLL) {
      return of(this.cacheCart);
    }

    return this.http.get<Product[]>(this.apiUrl).pipe(
      tap(items => {
      this.cacheCart = items;
      this.cartCacheTime = Date.now();
    }));
  }

  // Add an item to the cart
  addToCart(item: Product): Observable<Product[]> {
    var newItem: Product = { ...item }; 
    if(this.checkCartItem(item.id)){
      newItem = this.cacheCart!.find(item => item.id == newItem.id)!;
      newItem.stockQuantity += 1;
    }else{
      newItem.stockQuantity = 1;
    }
    return this.http.post<Product[]>(this.apiUrl, newItem).pipe(
      tap(items => {
        this.cacheCart = items;
        this.cartCacheTime = Date.now();
      }));
  }

  // Remove an item from the cart
  removeFromCart(id: number): Observable<Product[]> {
    return this.http.delete<Product[]>(`${this.apiUrl}/${id}`).pipe(
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
