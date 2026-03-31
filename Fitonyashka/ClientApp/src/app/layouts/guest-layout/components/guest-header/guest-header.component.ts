import { Component } from '@angular/core';

@Component({
  selector: 'app-guest-header',
  standalone: false,
  templateUrl: './guest-header.component.html',
  styleUrl: './guest-header.component.scss'
})
export class GuestHeaderComponent {
  menuOpen = false;

  toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }
}
