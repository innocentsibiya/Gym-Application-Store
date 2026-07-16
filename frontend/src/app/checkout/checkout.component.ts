import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Address } from '../Model/Address';
import { AddressService } from '../services/address.service';
import { CartService } from '../services/cart.service';
import { CartItemDto } from '../Model/CartItemDto';
import { CartDto } from '../Model/CartDto';
type CheckoutStep = 1 | 2 | 3;
type PaymentMethod = 'card' | 'eft';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.less']
})
export class CheckoutComponent implements OnInit {

  // ===== STATE =====
  step: CheckoutStep = 1;

  addressForm!: FormGroup;
  paymentForm!: FormGroup;

  addresses: Address[] = [];
  selectedAddressId: number | null = null;

  cartItems: CartItemDto[] = [];

  constructor(
    private fb: FormBuilder,
    private addressService: AddressService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.initForms();
    this.loadAddresses();
    this.handlePaymentChanges();
    this.cartService.loadCart().subscribe((cartItem: CartDto) => {
      this.cartItems = cartItem.items;
    });
  }

  private initForms() {
    this.addressForm = this.fb.group({
      fullName: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      postalCode: ['', Validators.required]
    });

    this.paymentForm = this.fb.group({
      method: ['card' as PaymentMethod, Validators.required],
      cardNumber: [''],
      cvv: [''],
      bankName: [''],
      accountNumber: ['']
    });
  }

  private handlePaymentChanges() {
    this.paymentForm.get('method')?.valueChanges.subscribe((method: PaymentMethod) => {
      this.clearPaymentValidators();

      if (method === 'card') {
        this.paymentForm.get('cardNumber')?.setValidators([Validators.required, Validators.minLength(12)]);
        this.paymentForm.get('cvv')?.setValidators([Validators.required, Validators.minLength(3)]);
      }

      if (method === 'eft') {
        this.paymentForm.get('bankName')?.setValidators([Validators.required]);
        this.paymentForm.get('accountNumber')?.setValidators([Validators.required]);
      }

      this.paymentForm.updateValueAndValidity();
    });
  }

  private clearPaymentValidators() {
    ['cardNumber', 'cvv', 'bankName', 'accountNumber'].forEach(field => {
      this.paymentForm.get(field)?.clearValidators();
      this.paymentForm.get(field)?.updateValueAndValidity({ emitEvent: false });
    });
  }

  loadAddresses() {
    this.addressService.getAddresses().subscribe({
      next: (res) => this.addresses = res,
      error: (err) => console.error('Failed to load addresses', err)
    });
  }

  selectAddress(id: number) {
    this.selectedAddressId = id;
  }

  addNewAddress() {
    if (this.addressForm.invalid) {
      this.addressForm.markAllAsTouched();
      return;
    }

    this.addressService.addAddress(this.addressForm.value)
      .subscribe({
        next: () => {
          this.loadAddresses();
          this.addressForm.reset();
        },
        error: (err) => console.error('Failed to add address', err)
      });
  }

  nextStep() {
    if (this.step === 1 && !this.selectedAddressId) return;

    if (this.step === 2 && this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      return;
    }

    this.step = (this.step + 1) as CheckoutStep;
  }

  prevStep() {
    this.step = (this.step - 1) as CheckoutStep;
  }

  get total(): number {
    return this.cartItems.reduce(
      (sum, item) => sum + item.price * item.quantity,
      0
    );
  }

  get selectedAddress(): Address | undefined {
    return this.addresses.find(a => a.Id === this.selectedAddressId!);
  }

  placeOrder() {
    if (!this.selectedAddressId || this.paymentForm.invalid) return;

    const payload = {
      address: this.selectedAddress,
      payment: this.paymentForm.value,
      items: this.cartItems,
      total: this.total
    };
  }
}