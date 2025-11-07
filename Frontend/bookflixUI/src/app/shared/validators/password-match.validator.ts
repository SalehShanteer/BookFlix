import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const PasswordMatchValidator : ValidatorFn = (control : AbstractControl): ValidationErrors | null => {
  const password = control.get('newPassword');
  const confirmPassword = control.get('confirmPassword');
  return password && confirmPassword && password.value !== confirmPassword.value ? { passwordMismatch: true }
    : null;
}

