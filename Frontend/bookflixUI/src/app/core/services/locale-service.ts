import { Injectable } from '@angular/core';
import { DirectionService } from './direction-service';
import localeEnData from '../../../assets/Locale/LocaleEn.json';
import localeArData from '../../../assets/Locale/LocaleAr.json';

@Injectable({
  providedIn: 'root',
})
export class LocaleService {
  private currentLanguage: string = 'en';
  private localeEn: Record<string, string> = localeEnData;
  private localeAr: Record<string, string> = localeArData;
  constructor(private directionService: DirectionService) {}

  loadLanguage(): Promise<void> {
    return new Promise((resolve) => {
      const savedLanguage = localStorage.getItem('language');
      this.currentLanguage = savedLanguage ?? 'en';
      this.directionService.setDirection(this.currentLanguage);
      console.log('Loaded language:', this.currentLanguage);
      resolve();
    });
  }

  getLocale(key: string): string {
    return this.currentLanguage === 'ar' ? this.localeAr[key] ?? key : this.localeEn[key] ?? key;
  }
}
