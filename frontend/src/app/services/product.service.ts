import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import {
  BehaviorSubject,
  Observable,
  of
} from 'rxjs';

import {
  catchError,
  tap
} from 'rxjs/operators';

import { Product } from '../interface/Product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly baseUrl = 'http://localhost:5074/api/Products';

  private productsSubject = new BehaviorSubject<Product[]>([]);

  public products$ = this.productsSubject.asObservable();

  private loaded = false;

  constructor(private http: HttpClient) { }

  loadProducts(forceRefresh = false): Observable<Product[]> {

    if (this.loaded && !forceRefresh) {
      return of(this.productsSubject.value);
    }

    return this.http.get<Product[]>(this.baseUrl).pipe(
      tap(products => {
        this.productsSubject.next(products);
        this.loaded = true;
      }),
      catchError(error => {
        console.error('Failed to load products', error);
        return of([]);
      })
    );
  }

  getAllProducts(forceRefresh = false): Observable<Product[]> {
    return this.loadProducts(forceRefresh);
  }

  getProducts(): Product[] {
    return this.productsSubject.value;
  }

  getProductById(id: number): Observable<Product> {

    const cachedProduct =
      this.productsSubject.value.find(
        product => product.id === id
      );

    if (cachedProduct) {
      return of(cachedProduct);
    }

    return this.http.get<Product>(
      `${this.baseUrl}/${id}`
    );
  }

  searchProducts(
    term: string,
    page: number = 1,
    pageSize: number = 6
  ): Observable<any> {

    const url =
      `${this.baseUrl}/search` +
      `?term=${encodeURIComponent(term)}` +
      `&page=${page}` +
      `&pageSize=${pageSize}`;

    return this.http.get<any>(url);
  }

  refreshProducts(): Observable<Product[]> {
    return this.loadProducts(true);
  }

  clearCache(): void {
    this.loaded = false;
    this.productsSubject.next([]);
  }
}