import { Component } from '@angular/core';
import { LocalePipe } from '../../../shared/pipes/locale-pipe';

@Component({
  selector: 'app-account-settings',
  imports: [LocalePipe],
  templateUrl: './account-settings.html',
  styleUrl: './account-settings.scss'
})
export class AccountSettings {

}
