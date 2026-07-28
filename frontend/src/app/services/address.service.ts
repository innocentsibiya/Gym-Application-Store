import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AddressDto } from '../Model/AddressDto';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private apiUrl = 'http://localhost:5074/api/Address';

  constructor(private http: HttpClient) {}

  getUserAddresses(userId: number): Observable<AddressDto[]> {
    return this.http.get<AddressDto[]>(`${this.apiUrl}/${userId}`);
  }

  addAddress(address: AddressDto): Observable<any> {
    return this.http.post(this.apiUrl, address).pipe(
      catchError(err => throwError(() => err.error?.message || 'Failed to add address'))
    );
  }

  updateAddress(address: AddressDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${address.id}`, address).pipe(
      catchError(err => throwError(() => err.error?.message || 'Failed to update address'))
    );
  }

  removeAddress(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError(err => throwError(() => err.error?.message || 'Failed to remove address'))
    );
  }
}