import { HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from '../services/account/account.service';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private auth: AccountService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    const token = this.auth.getToken();
    console.info(token + ' 111');
    if (!token) return next.handle(req);

    return next.handle(
      req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    );
  }
}
