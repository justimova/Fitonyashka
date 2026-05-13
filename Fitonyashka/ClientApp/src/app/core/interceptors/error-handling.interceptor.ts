import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ErrorHandlingService } from '../services/error-handling.service';
import { AccountService } from '../services/account/account.service';

@Injectable()
export class ErrorHandlingInterceptor implements HttpInterceptor {
  constructor(
    private errorHandlingService: ErrorHandlingService,
    private router: Router,
    private accountService: AccountService
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorCode = 'HTTP_ERROR';
        let message = 'An error occurred while processing your request';

        if (error.status === 401) {
          errorCode = 'UNAUTHORIZED';
          message = 'Your session has expired. Please log in again.';
          this.accountService.logout();
          this.router.navigate(['/auth/login']);
        } else if (error.status === 403) {
          errorCode = 'FORBIDDEN';
          message = 'You do not have permission to perform this action.';
        } else if (error.status === 404) {
          errorCode = 'NOT_FOUND';
          message = 'The requested resource was not found.';
        } else if (error.status === 500) {
          errorCode = 'SERVER_ERROR';
          message = 'Server error. Please try again later.';
        } else if (error.status === 0) {
          errorCode = 'NETWORK_ERROR';
          message = 'Network error. Please check your connection.';
        } else if (error.error?.message) {
          message = error.error.message;
        }

        this.errorHandlingService.handleError(
          { error: error.error, message },
          errorCode
        );

        return throwError(() => error);
      })
    );
  }
}
