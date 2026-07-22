import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddressService } from '../../services/address.service';
import { Address } from '../../Model/Address';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.less']
})
export class AddressComponent implements OnInit {

  @Output() addressSelected = new EventEmitter<Address>();

  addresses: Address[] = [];
  selectedAddressId: number | null = null;

  addressForm!: FormGroup;

  editing = false;
  editingId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private addressService: AddressService
  ) {}

  ngOnInit(): void {
    this.addressForm = this.fb.group({
      fullName: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required]
    });

    this.loadAddresses();
  }

  loadAddresses(): void {
    this.addressService.getAddresses().subscribe({
      next: (res: Address[]) => this.addresses = res,
      error: (err: any) => console.error(err)
    });
  }

  selectAddress(address: Address): void {
    this.selectedAddressId = address.id;
    this.addressSelected.emit(address);
  }

  addOrUpdateAddress(): void {

    if (this.addressForm.invalid) {
      this.addressForm.markAllAsTouched();
      return;
    }

    if (this.editing) {

      const payload = {
        id: this.editingId,
        ...this.addressForm.value
      };

      this.addressService.updateAddress(payload).subscribe({
        next: () => {
          this.cancelEdit();
          this.loadAddresses();
        }
      });

      return;
    }

    this.addressService.addAddress(this.addressForm.value).subscribe({
      next: () => {
        this.addressForm.reset();
        this.loadAddresses();
      }
    });

  }

  editAddress(address: Address): void {

    this.editing = true;
    this.editingId = address.id;

    this.addressForm.patchValue({
      fullName: address.fullName,
      city: address.city,
      postalCode: address.postalCode
    });

  }

  cancelEdit(): void {
    this.editing = false;
    this.editingId = null;
    this.addressForm.reset();
  }

}