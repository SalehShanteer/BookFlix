import { Component } from '@angular/core';
import { LocalePipe } from '../../pipes/locale-pipe';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-server-error',
  imports: [LocalePipe],
  templateUrl: './server-error.html',
  styleUrl: './server-error.scss',
})
export class ServerError {
  constructor(private router: Router, private location: Location) {}
  goHome() {
    this.router.navigate(['/']);
  }
  goBack() {
    this.location.back();
  }
}
