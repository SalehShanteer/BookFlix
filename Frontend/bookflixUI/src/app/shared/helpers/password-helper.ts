export class PasswordHelper {
  public static IsStrongPassword(password: string): boolean {
    if (password.length < 8) return false;
    let hasUpper: boolean = false,
      hasLower: boolean = false,
      hasDigit: boolean = false,
      hasSpecial: boolean = false;

    for (const c of password) {
      if (/[A-Z]/.test(c)) hasUpper = true;
      else if (/[a-z]/.test(c)) hasLower = true;
      else if (/[0-9]/.test(c)) hasDigit = true;
      else hasSpecial = true;

      if (hasUpper && hasLower && hasDigit && hasSpecial) {
        return true;
      }
    }

    return false;
  }
}
