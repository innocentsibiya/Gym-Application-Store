import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GymItem } from './gym-item-service.service';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CartService {
private apiUrl = 'http://localhost:5074/api/Cart';  

  constructor(private http: HttpClient) { }

  // Get cart items from the API
  getCart(): Observable<GymItem[]> {
    return this.http.get<GymItem[]>(this.apiUrl);
  }

  // Add an item to the cart
  addToCart(item: GymItem): Observable<GymItem[]> {
    return this.http.post<GymItem[]>(this.apiUrl, item);
  }

  // Remove an item from the cart
  removeFromCart(id: number): Observable<GymItem[]> {
    return this.http.delete<GymItem[]>(`${this.apiUrl}/${id}`);
  }
}
