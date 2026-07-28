import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddressDto } from '../Model/AddressDto';
import { AddressService } from '../services/address.service';
import { CartService } from '../services/cart.service';
import { CartItemDto } from '../Model/CartItemDto';
import { CartDto } from '../Model/CartDto';
import { Router } from '@angular/router';

type CheckoutStep = 0 | 1 | 2 | 3;
type PaymentMethod = 'card' | 'eft';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.less']
})
export class CheckoutComponent implements OnInit {

  step: CheckoutStep = 1;
  paymentForm!: FormGroup;

  addresses: AddressDto[] = [];
  selectedAddressId: number | null = null;
  selectedAddress?: AddressDto;

  cartItems: CartItemDto[] = [];
  message: string = '';
  total = 0;
  showAddressModal = false;
  editingAddress?: AddressDto;

  constructor(
    private fb: FormBuilder,
    private addressService: AddressService,
    private cartService: CartService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadAddresses();
    this.loadCart();
  }

  handlePaymentSubmit(form: FormGroup): void {
    this.paymentForm = form;
    this.nextStep();
  }

  loadAddresses(): void {
    this.addressService.getUserAddresses(1).subscribe({
      next: (data) => this.addresses = data,
      error: (err) => this.message = err
    });
  }

  loadCart(): void {
    this.cartService.loadCart().subscribe((cartItem: CartDto) => {
      this.cartItems = cartItem.items;
      this.total = this.cartItems.reduce((sum, item) => sum + item.price * item.quantity, 0);
    });
  }

  selectAddress(id: number) {
    this.selectedAddressId = id;
    this.selectedAddress = this.addresses.find(a => a.id === id) || undefined;
  }

  openAddAddressModal() {
    this.editingAddress = undefined;
    this.showAddressModal = true;
  }

  openEditAddressModal(address: AddressDto) {
    this.editingAddress = address;
    this.showAddressModal = true;
  }

  closeAddressModal() {
    this.showAddressModal = false;
    this.editingAddress = undefined;
    this.loadAddresses();
  }

  nextStep() {
    if (this.step === 1 && !this.selectedAddress) return;
    if (this.step === 2 && this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      return;
    }
    this.step = (this.step + 1) as CheckoutStep;
  }

  prevStep() {
    this.step = (this.step - 1) as CheckoutStep;
    if (this.step === 0) {
      this.router.navigate(['/cart']);
    }
  }

  placeOrder() {
    if (!this.selectedAddressId || this.paymentForm.invalid) return;

    const payload = {
      address: this.selectedAddress,
      payment: this.paymentForm.value,
      items: this.cartItems,
      total: this.total
    };

    console.log('Order placed:', payload);
    // TODO: send to backend
  }
}