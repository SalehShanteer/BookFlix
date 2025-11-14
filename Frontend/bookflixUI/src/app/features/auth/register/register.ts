import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth-service';
import { Router } from '@angular/router';
import { ISignup } from '../../../core/models/auth/signup.model';
import { PasswordMatchValidator } from '../../../shared/validators/password-match.validator';
import { LocaleService } from '../../../core/services/locale-service';
import { LocalePipe } from '../../../shared/pipes/locale-pipe';
import { BaseComponent } from '../../../shared/base/base-component';
import { ErrorHelper } from '../../../shared/helpers/error-helper';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, LocalePipe],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register extends BaseComponent {
  registerForm!: FormGroup;

  usernameError: string | null = null;
  usernameHasError: boolean = false;

  emailError: string | null = null;
  emailHasError: boolean = false;

  passwordError: string | null = null;
  passwordHasError: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private localeService: LocaleService
  ) {
    super();
    this.onLoadModel();
  }

  override onLoadModel(): void {
    this.registerForm = this.fb.group(
      {
        username: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        newPassword: ['', Validators.required],
        confirmPassword: ['', Validators.required],
      },
      { validators: PasswordMatchValidator }
    );
    this.loadErrorMessages();
  }

  private resetErrors() {
    this.usernameHasError = false;
    this.emailHasError = false;
    this.passwordHasError = false;
  }

  private loadErrorMessages() {
    this.usernameError = this.localeService.getLocale('UsernameUsed');
    this.emailError = this.localeService.getLocale('EmailUsed');
    this.passwordError = this.localeService.getLocale('PasswordWeak');
  }

  private errorHandling(errors: string[]) {
    if (errors.includes('UsernameUsed')) this.usernameHasError = true;
    if (errors.includes('EmailUsed')) this.emailHasError = true;
    if (errors.includes('PasswordWeak')) this.passwordHasError = true;
  }

  private showDashboardScreen() {
    console.log('register successful');
    this.router.navigate(['/dashboard']);
  }

  onSignUp() {
    this.resetErrors();

    const { username, email, newPassword } = this.registerForm.value;
    const registerRequest: ISignup = {
      username,
      email,
      password: newPassword,
    };
    this.authService.signup(registerRequest).subscribe({
      next: () => {
        this.showDashboardScreen();
      },
      error: (err) => {
        let errorsArray: string[] = ErrorHelper.toArray(err);
        this.errorHandling(errorsArray);
      },
    });
  }
}
