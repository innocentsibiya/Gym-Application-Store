import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less'] 
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  submitted = false;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.registerForm = this.fb.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: [
          '',
          [
            Validators.required,
            Validators.pattern(
              /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$/
            ),
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      {
        validators: this.passwordMatchValidator(), 
      }
    );
  }

  passwordMatchValidator(): ValidatorFn {
    return (group: AbstractControl) => {
      const password = group.get('password')?.value;
      const confirmPassword = group.get('confirmPassword')?.value;
      if (password !== confirmPassword) {
        group.get('confirmPassword')?.setErrors({ mismatch: true });
      } else {
        const errors = group.get('confirmPassword')?.errors;
        if (errors) {
          delete errors['mismatch'];
          if (Object.keys(errors).length === 0) {
            group.get('confirmPassword')?.setErrors(null);
          } else {
            group.get('confirmPassword')?.setErrors(errors);
          }
        }
      }
      return null; 
    };
  }

  get firstName(): AbstractControl {
    return this.registerForm.get('firstName')!;
  }

  get lastName(): AbstractControl {
    return this.registerForm.get('lastName')!;
  }

  get email(): AbstractControl {
    return this.registerForm.get('email')!;
  }

  get password(): AbstractControl {
    return this.registerForm.get('password')!;
  }

  get confirmPassword(): AbstractControl {
    return this.registerForm.get('confirmPassword')!;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.registerForm.invalid) {
      return;
    }

    alert('Form Submitted:\n\n' + JSON.stringify(this.registerForm.value, null, 2));
  }

  onReset(): void {
    this.submitted = false;
    this.registerForm.reset();
  }
}