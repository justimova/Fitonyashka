import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { IUserInfo } from 'src/app/core/models/account/user';
import { AccountService } from 'src/app/core/services/account/account.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  menuOpen = false;
  userMenuOpen = false;

  userName = 'User';
  userAvatar = 'assets/default-avatar.png';

  constructor(
    private router: Router,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    const storedUser = this.accountService.getUser();
    if (storedUser) {
      this.applyUser(storedUser);
    } else if (this.accountService.isLoggedIn()) {
      this.accountService.fetchCurrentUser().subscribe({
        next: (user) => this.applyUser(user)
      });
    }

    this.accountService.currentUser$.subscribe(user => {
      if (user) {
        this.applyUser(user);
      }
    });
  }

  @HostListener('document:click')
  onDocumentClick(): void {
    this.userMenuOpen = false;
  }

  private applyUser(user: IUserInfo): void {
    this.userName = user.firstName || user.username || 'Пользователь';
    this.userAvatar = user.avatarFileName
      ? `assets/UserImages/${user.avatarFileName}`
      : 'assets/default-avatar.png';
  }

  toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }

  closeMenu(): void {
    this.menuOpen = false;
  }

  toggleUserMenu(event: MouseEvent): void {
    event.stopPropagation();
    this.userMenuOpen = !this.userMenuOpen;
  }

  closeUserMenu(): void {
    this.userMenuOpen = false;
  }

  navigate(path: string): void {
    this.router.navigate([path]);
    this.closeMenu();
    this.closeUserMenu();
  }

  logout(): void {
    this.accountService.logout();
    this.router.navigate(['/auth/login']);
    this.closeUserMenu();
  }
}
