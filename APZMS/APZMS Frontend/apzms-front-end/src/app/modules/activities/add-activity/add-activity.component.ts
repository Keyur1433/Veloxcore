import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { ActivityService } from '../../../core/services/activity/activity.service';

@Component({
  selector: 'app-add-activity',
  standalone: false,
  templateUrl: './add-activity.component.html',
  styleUrls: ['./add-activity.component.css']
})
export class AddActivityComponent implements OnInit, OnDestroy {
  activityForm!: FormGroup;
  selectedFile: File | null = null
  error = ''
  isLoading: boolean = false;
  hasSearched = false;

  private destroy$ = new Subject<void>()

  constructor(private activityService: ActivityService) { }

  ngOnInit() {
    this.activityForm = this.createActivityForm();
  }

   private createActivityForm(): FormGroup {
    return new FormGroup({
      name: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      price: new FormControl('', [Validators.required, Validators.min(0)]),
      capacity: new FormControl('', [Validators.required, Validators.min(0)]),
      minAge: new FormControl('', [Validators.required, Validators.min(2), Validators.max(99)]),
      maxAge: new FormControl('', Validators.required),
      safetyLevel: new FormControl('', [Validators.required, Validators.pattern(/^(low|medium|high)$/)]),
    });
  }

  onSubmit() {
    if (this.activityForm.valid) {
      const formValues = this.activityForm.value;

      // Create FormData for multipart/form-data
      const formData = new FormData()

      // Append all form fields to FormData
      formData.append('Name', formValues.name || '');
      formData.append('Description', formValues.description || '');
      formData.append('Price', formValues.price?.toString() || '0');
      formData.append('Capacity', formValues.capacity?.toString() || '0');
      formData.append('MinAge', formValues.minAge?.toString() || '0');
      formData.append('MaxAge', formValues.maxAge?.toString() || '0');
      formData.append('SafetyLevel', formValues.safetyLevel || '');

      // Add null check before appending file
      if (this.selectedFile) {
        formData.append('Photo', this.selectedFile, this.selectedFile.name);
      }

      this.activityService.addActivity(formData)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          alert("Activity added successfully")
          console.log("Activity added successfully: ", response);
          this.activityForm.reset();
        },
        error: (err) => {
          console.error("Error: ", err);
          this.error = 'Failed to add activity. Please try again.';
        }
      });
    } else {
      this.activityForm.markAllAsTouched();
    }
  }

  // Handle file selection
  onFileSelected(event: any): void {
    const file = event.target.files[0]

    if (file) {
      this.selectedFile = file
      this.activityForm.patchValue({ photo: file })
      this.activityForm.get('photo')?.updateValueAndValidity()
    } else {
      this.selectedFile = null
      this.activityForm.patchValue({ photo: null })
    }
  }

  getFieldError(fieldName: string, errorType: string): boolean {
    if (!this.activityForm) {
      return false;
    }
    const field = this.activityForm.get(fieldName);
    return !!(field && field.hasError(errorType) && (field.dirty || field.touched));
  }

  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }
}