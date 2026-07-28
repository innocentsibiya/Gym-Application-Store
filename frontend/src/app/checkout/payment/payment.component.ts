import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

type PaymentMethod = 'card' | 'eft';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.less']
})
export class PaymentComponent implements OnInit {
  paymentForm!: FormGroup;

  @Output() submit = new EventEmitter<FormGroup>();
  @Output() back = new EventEmitter<void>();

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.paymentForm = this.fb.group({
      method: ['card' as PaymentMethod, Validators.required],
      cardNumber: [''],
      cvv: [''],
      bankName: [''],
      accountNumber: ['']
    });

    this.paymentForm.get('method')?.valueChanges.subscribe((method: PaymentMethod) => {
      this.clearValidators();

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

  private clearValidators() {
    ['cardNumber', 'cvv', 'bankName', 'accountNumber'].forEach(field => {
      this.paymentForm.get(field)?.clearValidators();
      this.paymentForm.get(field)?.updateValueAndValidity({ emitEvent: false });
    });
  }

  submitPayment() {
    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      return;
    }
    this.submit.emit(this.paymentForm);
  }
}