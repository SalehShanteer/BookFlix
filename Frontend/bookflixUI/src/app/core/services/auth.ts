import { Injectable } from '@angular/core';
import { Api } from './api';
import { ISignup } from '../models/auth/signup.model';
import { IToken } from '../models/auth/token.model';
import { Observable, tap } from 'rxjs';
import { TokenHelper } from '../../shared/helpers/token.helper';
import { ILogin } from '../models/auth/login.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private api: Api) {}

  login(loginModel: ILogin): Observable<IToken> {
    return this.api.post<IToken>('/auth/login', loginModel).pipe(
      tap((res) => {
        TokenHelper.setAccessToken(res.accessToken);
        TokenHelper.setRefreshToken(res.refreshToken);
      })
    );
  }

  signup(signupModel: ISignup): Observable<IToken> {
    return this.api.post<IToken>('/auth/signup', signupModel).pipe(
      tap((res) => {
        TokenHelper.setAccessToken(res.accessToken);
        TokenHelper.setRefreshToken(res.refreshToken);
      })
    );
  }
}
