import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { IUserLogin, IUserRegister, IUserInfo } from '../../models/account/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private tokenKey: string = 'auth_token';

  constructor(
    private http: HttpClient,
  ) { }

  public getCurrentUser(): Observable<IUserInfo> {
    return this.http.get<IUserInfo>('/api/account/currentUser') as Observable<IUserInfo>;
  }

  public register(user: IUserRegister): Observable<any> {
    return this.http.post('/api/account', user);
  }

  public login(user: IUserLogin): Observable<any> {
    return this.http.post<{ token: string }>('/api/account/login', user)
      .pipe(
        tap(r => localStorage.setItem(this.tokenKey, r.token)),
        map(() => void 0)
      );
  }

  public logout() {
    localStorage.removeItem(this.tokenKey);
  }

  public getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  public isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
