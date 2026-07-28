import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddressService } from '../../services/address.service';
import { AddressDto } from '../../Model/AddressDto';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.less']
})
export class AddressComponent implements OnInit {

  @Input() editingAddress?: AddressDto;
  @Output() addressSelected = new EventEmitter<AddressDto>();
  @Output() cancel = new EventEmitter<void>();

  addresses: AddressDto[] = [];
  selectedAddressId: number | null = null;

  addressForm!: FormGroup;

  editing = false;
  editingId: number | null = null;
  message: string = '';

  constructor(
    private fb: FormBuilder,
    private addressService: AddressService
  ) { }

  ngOnInit(): void {
  this.addressForm = this.fb.group({
    street: ['', Validators.required],
    city: ['', Validators.required],
    province: ['', Validators.required],
    postalCode: ['', Validators.required],
    country: ['', Validators.required],
    addressType: ['Shipping', Validators.required],
    isDefault: [false]
  });

  if (this.editingAddress) {
    this.editing = true;
    this.editingId = this.editingAddress.id;
    this.addressForm.patchValue(this.editingAddress);
  }
  }

  loadAddresses(): void {
    this.addressService.getUserAddresses(1).subscribe({
      next: (res: AddressDto[]) => this.addresses = res,
      error: (err: any) => console.error(err)
    });
  }

  selectAddress(address: AddressDto): void {
    this.selectedAddressId = address.id;
    this.addressSelected.emit(address);
  }

  addOrUpdateAddress(): void {
    if (this.addressForm.invalid) {
      this.addressForm.markAllAsTouched();
      return;
    }

    const payload: AddressDto = {
      id: this.editing ? this.editingId! : 0,
      userId: 1,
      ...this.addressForm.value
    };

    if (this.editing) {
      this.addressService.updateAddress(payload).subscribe({
        next: () => {
          this.message = 'Address updated successfully';
          this.cancelEdit();
          this.loadAddresses();
        },
        error: (err) => this.message = err
      });
    } else {
      this.addressService.addAddress(payload).subscribe({
        next: () => {
          this.message = 'Address added successfully';
          this.addressForm.reset();
          this.loadAddresses();
        },
        error: (err) => this.message = err
      });
    }
  }

  editAddress(address: AddressDto): void {
    this.editing = true;
    this.editingId = address.id;

    this.addressForm.patchValue({
      street: address.street,
      city: address.city,
      province: address.province,
      postalCode: address.postalCode,
      country: address.country,
      addressType: address.addressType,
      isDefault: address.isDefault
    });
  }
  cancelEdit(): void {
    this.editing = false;
    this.editingId = null;
    this.addressForm.reset();
    this.cancel.emit();
  }
}