import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DirectionService {
  setDirection(lang: string) {
    const dir = lang === 'ar' ? 'rtl' : 'ltr';
    document.documentElement.setAttribute('dir', dir);
    document.documentElement.setAttribute('lang', lang);
  }
}
