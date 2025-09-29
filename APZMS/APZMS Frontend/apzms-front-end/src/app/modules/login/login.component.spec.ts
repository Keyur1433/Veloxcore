import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { of, throwError } from 'rxjs'; // Import RxJS operators
import { LoginModule } from './login.module';
import { AuthService } from '../../core/services/auth.service';
import { LoginComponent } from './login.component';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockActivatedRoute: { snapshot: { queryParams: { returnUrl: null } } };

  beforeEach(async () => {
    // Create proper mocks
    mockAuthService = jasmine.createSpyObj('AuthService', ['login']);
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl']);
    mockActivatedRoute = { snapshot: { queryParams: { returnUrl: null } } };

    await TestBed.configureTestingModule({
      imports: [
        LoginModule,
        ReactiveFormsModule       // Required for reactive forms
      ],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter },
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the login form with required validators', () => {
    expect(component.loginForm).toBeDefined();
    const emailControl = component.loginForm.get('email');
    const passwordControl = component.loginForm.get('password');

    // Test empty form is invalid
    expect(component.loginForm.invalid).toBeTrue();
    expect(emailControl?.hasError('required')).toBeTrue();
    expect(passwordControl?.hasError('required')).toBeTrue();
  });

  it('should mark form as invalid when fields are empty', () => {
    component.onSubmit();
    expect(component.loginForm.valid).toBeFalse();
    expect(component.loginForm.get('email')?.touched).toBeTrue();
    expect(component.loginForm.get('password')?.touched).toBeTrue();
  });

  it('should call AuthService.login on valid form submission', fakeAsync(() => {
  // Create complete mock user object matching your User interface
  const mockUser = {
    id: '1',
    name: 'John Doe',
    email: 'staff.riya.bansal@example.com',
    role: 'staff',           // Add missing properties
    token: 'mock-jwt-token',
    expiresAt: Date.now() + 3600000  // 1 hour from now
  };
  
  mockAuthService.login.and.returnValue(of(mockUser));

  component.loginForm.setValue({
    email: 'staff.riya.bansal@example.com',
    password: 'string'
  });
  
  fixture.detectChanges();
  component.onSubmit();
  tick();

  expect(mockAuthService.login).toHaveBeenCalledWith({
    email: 'staff.riya.bansal@example.com',
    password: 'string'
  });
  expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/dashboard');
}));


  it('should set error on login failure', fakeAsync(() => {
    // Mock login error using throwError
    const errorResponse = { error: { message: 'Invalid credentials' } };
    mockAuthService.login.and.returnValue(throwError(() => errorResponse));

    component.loginForm.setValue({
      email: 'staff.riya.bansal@example.com', 
      password: 'string'
    });
    
    fixture.detectChanges();
    component.onSubmit();
    tick();
    fixture.detectChanges();

    // Check the actual error property name (adjust based on your component)
    expect(component.error).toBe('Invalid credentials'); // or whatever your error property is named
    // Alternative: expect(component.isSubmitting).toBeFalse();
  }));

  it('should disable submit button when form is invalid', () => {
    const submitButton = fixture.debugElement.nativeElement.querySelector('button[type="submit"]');
    
    expect(submitButton.disabled).toBeTrue();
    
    // Fill form with valid data
    component.loginForm.setValue({
      email: 'valid@example.com',
      password: 'validpassword'
    });
    fixture.detectChanges();
    
    expect(submitButton.disabled).toBeFalse();
  });

  it('should show loading state during submission', fakeAsync(() => {
    // Return a delayed observable
    // mockAuthService.login.and.returnValue(of({ id: '1' }).pipe(delay(100)));
    
    component.loginForm.setValue({
      email: 'test@example.com',
      password: 'password'
    });
    
    component.onSubmit();
    
    expect(component.isSubmitting).toBeTrue(); // Check loading state
    
    tick(100); // Complete the delayed observable
    
    expect(component.isSubmitting).toBeFalse();
  }));
});
