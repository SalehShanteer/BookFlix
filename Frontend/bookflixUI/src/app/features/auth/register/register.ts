import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth';
import { Router } from '@angular/router';
import { ISignup } from '../../../core/models/auth/signup.model';
import { PasswordMatchValidator } from '../../../shared/validators/password-match.validator';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  registerForm: FormGroup;
  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService) {
    this.registerForm = this.fb.group(
      {
        username: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        newPassword: ['', Validators.required],
        confirmPassword: ['', Validators.required],
      },
      { validators: PasswordMatchValidator }
    );
  }

  onSignUp() {
    const registerRequest: ISignup = {
      username: this.registerForm.get('username')?.value,
      email: this.registerForm.get('email')?.value,
      password: this.registerForm.get('newPassword')?.value,
    };
    this.authService.signup(registerRequest).subscribe({
      next: (res) => {
        console.log('register successful:', res);
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('register failed:', err);
      },
    });
  }
}
