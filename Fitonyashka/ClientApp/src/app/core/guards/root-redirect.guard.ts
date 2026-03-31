import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class RootRedirectGuard implements CanActivate {
  constructor(private router: Router, private accountService: AccountService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const isAuthenticated = this.accountService.isLoggedIn();

    if (isAuthenticated) {
      this.router.navigate(['/dashboard']);
    } else {
      this.router.navigate(['/home']);
    }

    return false;
  }
}
