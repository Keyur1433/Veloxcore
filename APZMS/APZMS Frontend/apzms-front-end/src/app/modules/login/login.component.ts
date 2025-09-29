import { ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnDestroy {
  loginForm!: FormGroup;
  loading = false;
  error: string | null = null;
  returnUrl: string = '/dashboard';
  isSubmitting!: boolean

  private destroy$ = new Subject<void>()

  constructor(
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {
    this.loginForm = new FormGroup({
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", [Validators.required]),
    })

    // read returnUrl if set by guard
    const q = route.snapshot.queryParams['returnUrl']
    if (q) this.returnUrl = q
  }

  onSubmit() {
    if (this.loginForm.invalid) return
    this.loading = true
    this.error = null

    this.auth.login(this.loginForm.value)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          // After successful login navigate to returnUrl
          this.router.navigateByUrl(this.returnUrl)
        },
        error: (err) => {
          // Map server-side 400/401 into friendly messages here
          this.error = (err?.error?.errorMessage) || 'Login failed. Check credentials.';
          this.loading = false;
          this.cdr.detectChanges(); // If the creds are wrong, then it manually tells Angular: “Hey, something changed -- here something is changed means that creds are wrong, so please update the view to show error message”
        }
      })
  }

  get email() {
    return this.loginForm.get('email')?.touched && this.loginForm.get('email')?.invalid
  }

  ngOnDestroy(): void {
    this.destroy$.next()
    this.destroy$.complete()
  }
}