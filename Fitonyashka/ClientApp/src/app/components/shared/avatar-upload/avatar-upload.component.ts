import { Component } from '@angular/core';
import { AccountService } from '../../../core/services/account/account.service';
import { AvatarService } from 'src/app/core/services/account/avatar.service';

@Component({
  selector: 'app-avatar-upload',
  standalone: false,
  templateUrl: './avatar-upload.component.html',
  styleUrl: './avatar-upload.component.scss'
})
export class AvatarUploadComponent {
  selectedFile: File | null = null;
  avatarUrl: string | null = null;
  uploadProgress = 0;

  constructor(private avatarService: AvatarService) { }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;

    if (!file) return;

    const allowed = ['image/jpeg', 'image/png', 'image/webp'];
    if (!allowed.includes(file.type)) {
      alert('Только JPG/PNG/WEBP');
      input.value = '';
      return;
    }

    if (file.size > 2 * 1024 * 1024) {
      alert('File too big (maximum 2 MB)');
      input.value = '';
      return;
    }

    this.selectedFile = file;
  }

  onUpload(file: File): void {
    this.avatarService.uploadAvatar(file).subscribe({
      // next: (state) => {
      //   this.uploadProgress = state.progress;

      //   if (state.isDone) {
      //     this.avatarUrl = state.avatarUrl;
      //   }
      // },
      // error: () => {
      //   alert('Upload error');
      //   this.uploadProgress = 0;
      // }
    });
  }
}
