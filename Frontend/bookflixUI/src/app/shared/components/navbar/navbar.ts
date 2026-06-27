import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { LocalePipe } from '../../pipes/locale-pipe';
import { TokenHelper } from '../../helpers/token-helper';

@Component({
  selector: 'app-navbar',
  imports: [LocalePipe, RouterModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss',
})
export class Navbar {
  constructor(private router: Router) {}
  signOut() {
    TokenHelper.removeTokens();
    this.router.navigate(['login']);
  }
}
