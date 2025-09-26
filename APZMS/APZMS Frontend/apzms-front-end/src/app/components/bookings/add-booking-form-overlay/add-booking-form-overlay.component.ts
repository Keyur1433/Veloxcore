import { NgIf } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-booking-form-overlay',
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './add-booking-form-overlay.component.html',
  styleUrls: ['./add-booking-form-overlay.component.css']
})

export class AddBookingFormOverlayComponent implements OnInit {
  @Input() isOpen = false
  @Output() closeModelEvent = new EventEmitter<void>()
  @Output() submitFormEvent = new EventEmitter<any>()

  addBookingForm!: FormGroup
  isSubmitting: boolean = false
  errorMessage!: string;

  constructor() { }

  ngOnInit() {
    this.addBookingForm = this.generateAddBookingsForm()
  }

  private generateAddBookingsForm(): FormGroup {
    return new FormGroup({
      bookingDate: new FormControl('', [
        Validators.required,
        this.futureDateValidator
      ]),
      timeSlot: new FormControl('', [
        Validators.required,
        Validators.pattern(/^([01]\d|2[0-3]):([0-5]\d)$/)
      ]),
      participants: new FormControl('', [
        Validators.required,
        Validators.min(1)
      ])
    });
  }

  closeModal() {
    this.addBookingForm.reset()
    this.errorMessage = '';
    this.isSubmitting = false
    this.closeModelEvent.emit()
  }

  onSubmit() {
    if (this.addBookingForm.valid) {
      this.isSubmitting = true
      this.errorMessage = '';

      // Emit form data to parent component
      this.submitFormEvent.emit(this.addBookingForm.value)
    } else {
      this.addBookingForm.markAllAsTouched();
    }
  }

  // Method to handle submission response from parent
  handleSubmissionResponse(success: boolean, errMessage?: string) {
    this.isSubmitting = false
    if (success) {
      this.closeModal()
    } else {
      this.errorMessage = errMessage || 'Submission failed. Please try again.'
    }
  }

  // Custom validator to check for future date
  futureDateValidator(control: AbstractControl) {
    const currentDate = new Date();
    const bookingDate = new Date(control.value);
    return bookingDate <= currentDate ? { futureDate: true } : null
  }

  getFieldError(fieldName: string, errorType: string): boolean {
    if (!this.addBookingForm) {
      return false;
    }
    const field = this.addBookingForm.get(fieldName);
    return !!(field && field.hasError(errorType) && (field.dirty || field.touched));
  }
}