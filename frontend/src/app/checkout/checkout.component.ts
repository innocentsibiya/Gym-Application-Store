import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Address } from '../Model/Address';
import { AddressService } from '../services/address.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.less']
})
export class CheckoutComponent implements OnInit {

  step = 1;

  addressForm!: FormGroup;
  paymentForm!: FormGroup;

  addresses: Address[] = [];
  selectedAddressId!: number;

  constructor(
    private fb: FormBuilder,
    private addressService: AddressService
  ) {}

  ngOnInit(): void {
    this.loadAddresses();

    this.addressForm = this.fb.group({
      fullName: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required]
    });

    this.paymentForm = this.fb.group({
      method: ['card', Validators.required],
      cardNumber: [''],
      cvv: [''],
      bankName: [''],
      accountNumber: ['']
    });
  }

  loadAddresses() {
    this.addressService.getAddresses().subscribe(res => {
      this.addresses = res;
    });
  }

  selectAddress(id: number) {
    this.selectedAddressId = id;
  }

  addNewAddress() {
    if (this.addressForm.invalid) return;

    this.addressService.addAddress(this.addressForm.value)
      .subscribe(() => {
        this.loadAddresses();
        this.addressForm.reset();
      });
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }

  placeOrder() {
    const payload = {
      addressId: this.selectedAddressId,
      payment: this.paymentForm.value
    };

    console.log('Final Order:', payload);
  }
}