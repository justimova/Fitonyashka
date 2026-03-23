import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  menuOpen = false;

  constructor(private router: Router) {}

  toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }

  closeMenu(): void {
    this.menuOpen = false;
  }

  navigate(path: string): void {
    this.router.navigate([path]);
    this.closeMenu();
  }

  logout(): void {
    // Clear authentication token
    localStorage.removeItem('auth_token');
    this.router.navigate(['/auth/login']);
  }
}
