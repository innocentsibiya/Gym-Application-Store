import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { Product } from '../interface/Product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly baseUrl = 'http://localhost:5074/api/Products';
  private cache: Product[] | null = null;

  constructor(private http: HttpClient) {}

  getAllProducts(forceRefresh = false): Observable<Product[]> {
    if (this.cache && !forceRefresh) {
      return of(this.cache);
    }

    return this.http.get<Product[]>(this.baseUrl).pipe(
      tap(products => (this.cache = products))
    );
  }

  clearCache() {
    this.cache = null;
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/${id}`);
  }

    searchProducts(term: string, page: number = 1, pageSize: number = 6): Observable<any> {
    const url = `${this.baseUrl}/search?term=${encodeURIComponent(term)}&page=${page}&pageSize=${pageSize}`;
    return this.http.get<any>(url); 
  }
}