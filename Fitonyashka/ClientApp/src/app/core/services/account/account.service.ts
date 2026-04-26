import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { IUserLogin, IUserRegister, IUserInfo } from '../../models/account/user';
import { LocalStorageService } from '../local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private tokenKey = 'auth_token';
  private userKey = 'auth_user';

  private currentUserSubject = new BehaviorSubject<IUserInfo | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private localStorage: LocalStorageService
  ) {
    const storedUser = this.localStorage.getObject<IUserInfo>(this.userKey);
    if (storedUser) {
      this.currentUserSubject.next(storedUser);
    }
  }

  public fetchCurrentUser(): Observable<IUserInfo> {
    return this.http.get<IUserInfo>('/api/userProfile/currentUser').pipe(
      tap(user => this.saveUser(user))
    );
  }

  public register(user: IUserRegister): Observable<any> {
    return this.http.post('/api/account', user);
  }

  public login(user: IUserLogin): Observable<any> {
    return this.http.post<{ token: string }>('/api/account/login', user)
      .pipe(
        tap(r => {
          this.localStorage.setItem(this.tokenKey, r.token);
        }),
        map(() => void 0)
      );
  }

  public logout(): void {
    this.localStorage.removeItem(this.tokenKey);
    this.clearUser();
  }

  public getToken(): string | null {
    return this.localStorage.getItem(this.tokenKey);
  }

  public isLoggedIn(): boolean {
    return !!this.getToken();
  }

  public saveUser(user: IUserInfo): void {
    this.localStorage.setObject(this.userKey, user);
    this.currentUserSubject.next(user);
  }

  public getUser(): IUserInfo | null {
    const user = this.currentUserSubject.value;
    if (user) {
      return user;
    }

    const fromStorage = this.localStorage.getObject<IUserInfo>(this.userKey);
    if (fromStorage) {
      this.currentUserSubject.next(fromStorage);
    }

    return fromStorage;
  }

  public clearUser(): void {
    this.localStorage.removeItem(this.userKey);
    this.currentUserSubject.next(null);
  }

  public getFirstName(): string | null {
    const user = this.getUser();
    return user?.firstName ?? null;
  }

  public getAvatarUrl(): string {
    const user = this.getUser();
    if (user && user.avatarFileName) {
      return `assets/UserImages/${user.avatarFileName}`;
    }

    return 'assets/default-avatar.png';
  }
}
