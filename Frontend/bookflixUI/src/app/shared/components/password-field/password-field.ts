import { Component, forwardRef, Input } from '@angular/core';
import { LocalePipe } from '../../pipes/locale-pipe';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-password-field',
  imports: [LocalePipe],
  templateUrl: './password-field.html',
  styleUrl: './password-field.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PasswordField),
      multi: true,
    },
  ],
})
export class PasswordField {
  @Input() placeholder: string = '';
  show: boolean = false;
  value = '';
  imageSource: string = 'assets/images/hide.png';
  onToggle() {
    this.show = !this.show;
    this.imageSource = this.show ? 'assets/images/show.png' : 'assets/images/hide.png';
  }
  onChange = (value: any) => {};
  onTouched = () => {};

  writeValue(value: any): void {
    this.value = value || '';
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  updateValue(value: string) {
    this.value = value;
    this.onChange(value);
  }
}
