import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { filter, map, Observable, startWith, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AvatarService {
  constructor(
    private http: HttpClient,
  ) { }

  uploadAvatar(avatarFile: File): Observable<string | null> {
    const formData = new FormData();
    formData.append('file', avatarFile);

    return this.http
      .post<{ avatarUrl: string }>('/api/avatar/uploadAvatar', formData)
      .pipe(
        map((response) => response.avatarUrl ?? null)
      );
  }
}
