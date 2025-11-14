import { Pipe, PipeTransform } from '@angular/core';
import { LocaleService } from '../../core/services/locale-service';

@Pipe({
  name: 'locale',
  pure: false,
})
export class LocalePipe implements PipeTransform {
  constructor(private localeService: LocaleService) {}
  transform(key: string): string {
    return this.localeService.getLocale(key);
  }
}
