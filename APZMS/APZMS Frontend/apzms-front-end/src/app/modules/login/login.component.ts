import { Component, OnDestroy, signal } from '@angular/core';
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
  loading = signal(false);
  error = signal<string | null>(null);
  returnUrl: string = '/dashboard';

  private destroy$ = new Subject<void>();

  constructor(
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
    });

    // Read returnUrl if set by guard
    const q = route.snapshot.queryParams['returnUrl'];
    if (q) this.returnUrl = q;
  }

  onSubmit() {
    if (this.loginForm.invalid) return;
    this.loading.set(true);
    this.error.set(null);

    this.auth.login(this.loginForm.value)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          // After successful login navigate to returnUrl
          this.router.navigateByUrl(this.returnUrl);
        },
        error: (err) => {
          // Map server-side 400/401 into friendly messages here
          this.error.set(err?.error?.errorMessage || 'Login failed. Check credentials.');
          this.loading.set(false);
        }
      });
  }

  get email() {
    return this.loginForm.get('email')?.touched && this.loginForm.get('email')?.invalid;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Animated border states
  emailFocused = false;
  emailBlurred = false;
  passwordFocused = false;
  passwordBlurred = false;

  onEmailFocus() {
    this.emailBlurred = false;
    this.emailFocused = true;
  }

  onEmailBlur() {
    this.emailFocused = false;
    this.emailBlurred = true;
    setTimeout(() => {
      this.emailBlurred = false;
    }, 600);
  }

  onPasswordFocus() {
    this.passwordBlurred = false;
    this.passwordFocused = true;
  }

  onPasswordBlur() {
    this.passwordFocused = false;
    this.passwordBlurred = true;
    setTimeout(() => {
      this.passwordBlurred = false;
    }, 600);
  }
}