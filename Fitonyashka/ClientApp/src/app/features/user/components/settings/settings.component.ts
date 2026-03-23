import { Component } from '@angular/core';

@Component({
  selector: 'app-settings',
  standalone: false,
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent {
  settings = {
    notifications: true,
    emailUpdates: false,
    darkMode: false,
    language: 'en',
    privacy: 'public'
  };

  onSave(): void {
    console.log('Settings saved:', this.settings);
    // Call API to save settings
  }
}
