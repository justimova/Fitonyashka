import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { IResult } from '../models/result';

export interface AppError {
  code: string;
  message: string;
  severity: 'error' | 'warning' | 'info';
  timestamp: Date;
  details?: any;
}

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlingService {
  private errorSubject = new Subject<AppError>();
  public errors$ = this.errorSubject.asObservable();

  private errorStack: AppError[] = [];
  private maxErrors = 10;

  public handleError(error: any, code?: string): void {
    const appError = this.formatError(error, code);

    // Add to stack
    this.errorStack.push(appError);
    if (this.errorStack.length > this.maxErrors) {
      this.errorStack.shift();
    }

    // Emit error
    this.errorSubject.next(appError);

    // Log to console in development
    console.error(`[${appError.code}] ${appError.message}`, appError.details);
  }

  public handleSuccess(message: string): void {
    const successMessage: AppError = {
      code: 'SUCCESS',
      message,
      severity: 'info',
      timestamp: new Date()
    };
    this.errorSubject.next(successMessage);
  }

  public handleResponse(response: IResult): void {
    if (response.isSuccess) {
      this.handleSuccess('Operation completed successfully');
    } else {
      this.handleError(response.errorMessage || 'An error occurred', 'RESPONSE_ERROR');
    }
  }

  private formatError(error: any, code?: string): AppError {
    let message = 'An unexpected error occurred';
    let details = error;

    if (typeof error === 'string') {
      message = error;
    } else if (error instanceof Error) {
      message = error.message;
      details = error.stack;
    } else if (error?.error?.message) {
      message = error.error.message;
      details = error.error;
    } else if (error?.message) {
      message = error.message;
    }

    return {
      code: code || 'UNKNOWN_ERROR',
      message,
      severity: 'error',
      timestamp: new Date(),
      details
    };
  }

  getErrorStack(): AppError[] {
    return [...this.errorStack];
  }

  clearErrors(): void {
    this.errorStack = [];
  }

  getLastError(): AppError | null {
    return this.errorStack[this.errorStack.length - 1] || null;
  }
}
