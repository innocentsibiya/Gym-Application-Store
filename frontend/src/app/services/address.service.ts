import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Address } from '../Model/Address';

@Injectable({ providedIn: 'root' })
export class AddressService {

  private addresses: Address[] = [
    {
        id: 1,
        userId: 1,
        fullName: 'test',
        street: '12 main street',
        city: 'Durban',
        province: 'KZN',
        country: 'South Africa',
        postalCode: '4001',
        addressType: 'Home',
        isDefault: true
    }
  ];

  getAddresses(): Observable<Address[]> {
    return of(this.addresses);
  }

  addAddress(address: Address): Observable<Address> {
    address.id = Date.now();
    this.addresses.push(address);
    return of(address);
  }

  updateAddress(address: Address): Observable<Address> {
    const index = this.addresses.findIndex(addr => addr.id === address.id);
    if (index !== -1) {
      this.addresses[index] = address;
    }
    return of(address);
  }
}