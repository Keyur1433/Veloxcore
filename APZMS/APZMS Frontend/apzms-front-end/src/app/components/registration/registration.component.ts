import { Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { UserRegisterDto } from '../../core/models/user.model';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule]
})
export class RegistrationComponent implements OnDestroy{
  registrationForm!: FormGroup
  isSubmitting: boolean = false
  apiError: string = ""

  private destroy$ = new Subject<void>()

  constructor(private auth: AuthService, private router: Router) {
    this.registrationForm = this.createForm()
  }

  createForm(): FormGroup {
    return this.registrationForm = new FormGroup({
      name: new FormControl("", Validators.required),
      email: new FormControl("", Validators.required),
      phone: new FormControl("", [Validators.required, Validators.pattern(/^\+?[\d\s-()]+$/)]),
      password: new FormControl("", Validators.required),
      birthDate: new FormControl("", Validators.required),
    })
  }

  private formatDateForAPI(dateString: string): string {
    // Convert HTML date input (YYYY-MM-DD) to ISO DateTime format
    const date = new Date(dateString)
    return date.toISOString()
  }

  onSubmit(): void {
    if (this.registrationForm.valid) {
      this.isSubmitting = true;
      this.apiError = '';
    }

    const formData = this.registrationForm.value

    const userData: UserRegisterDto = {
      ...formData,
    }

    this.auth.register(userData)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (response) => {
        console.log('Registration successful:', response);

        alert("Register successful, click Ok to redirect to login page")
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.handleApiError(error)
      },
      complete: () => {
        this.isSubmitting = false;
      }
    })
  }

  private handleApiError(err: any) {
    if (err.status === 400 && err.error?.message) {
      this.apiError = err.error.message
    } else if (err.status === 409) {
      this.apiError = 'User already exists with this email';
    } else {
      this.apiError = 'Registration failed. Please try again.';
    }
  }

  getFieldError(fieldName: string, errorType: string): boolean {
    const field = this.registrationForm.get(fieldName)
    // field?.hasError(errorType) returns true or false
    // (field?.dirty || field?.touched) returns true or false
    // The whole expression becomes either true, false, or possibly undefined
    // !!(...) ensures the final returned value is exactly true or false (never undefined or any other type).
    return !!(field?.hasError(errorType) && (field?.dirty || field?.touched))
  }

  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }
}
