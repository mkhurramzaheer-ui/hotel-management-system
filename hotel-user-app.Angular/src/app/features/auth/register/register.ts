import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class RegisterComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  readonly registerForm = this.formBuilder.nonNullable.group({
    firstName: ['', [Validators.required]],
    lastName: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', [Validators.required]],
  });

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.authService.registerCustomer(this.registerForm.getRawValue()).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.successMessage =
          'Customer record created successfully. You can now go to the login page.';

        setTimeout(() => {
          void this.router.navigate(['/login']);
        }, 1200);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage =
          'Registration failed. Confirm the API is running and that the customer endpoint accepts this payload.';
      },
    });
  }
}
