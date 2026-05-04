import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Address } from '../Model/Address';

@Injectable({ providedIn: 'root' })
export class AddressService {

  private addresses: Address[] = [
    {
        Id: 1,
        UserId: 1,
        FullName: 'test',
        Street: '12 main street',
        City: 'Durban',
        Province: 'KZN',
        Country: 'South Africa',
        PostalCode: '4001',
        AddressType: 'Home',
        IsDefault: true
    }
  ];

  getAddresses(): Observable<Address[]> {
    return of(this.addresses);
  }

  addAddress(address: Address): Observable<Address> {
    address.Id = Date.now();
    this.addresses.push(address);
    return of(address);
  }
}