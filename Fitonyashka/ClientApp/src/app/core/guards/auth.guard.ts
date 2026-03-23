import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    // Check if user is authenticated
    const isAuthenticated = true; // !!localStorage.getItem('auth_token'); // Adjust based on your auth storage

    if (isAuthenticated) {
      return true;
    }

    // Redirect to login if not authenticated
    this.router.navigate(['/auth']);
    return false;
  }
}
