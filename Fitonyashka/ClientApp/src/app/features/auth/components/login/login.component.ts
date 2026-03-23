import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ErrorHandlingService } from 'src/app/core/services/error-handling.service';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { AccountService } from 'src/app/core/services/account/account.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  protected isLoading: boolean = false;
  protected showPassword: boolean = false;

  protected loginForm = this.formBuilder.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor(
    private router: Router,
    private errorHandlingService: ErrorHandlingService,
    private formBuilder: UntypedFormBuilder,
    private accountService: AccountService,
  ) { }

  protected togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onLogin(): void {
    this.isLoading = true;
    this.accountService.login(this.loginForm.value).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorHandlingService.handleError(error, 'LOGIN_ERROR');
      }
    });

    //// Reset error message
    //this.errorMessage = '';

    //// Validation
    //if (!this.username.trim() || !this.password.trim()) {
    //  this.errorMessage = 'Username and password are required';
    //  return;
    //}

    //if (this.username.length < 3) {
    //  this.errorMessage = 'Username must be at least 3 characters long';
    //  return;
    //}

    //if (this.password.length < 6) {
    //  this.errorMessage = 'Password must be at least 6 characters long';
    //  return;
    //}

    //this.isLoading = true;

    //// Simulate login - Replace with actual API call
    //setTimeout(() => {
    //  try {
    //    // Store auth token in localStorage
    //    localStorage.setItem('auth_token', 'fake-jwt-token-' + Date.now());
    //    localStorage.setItem('username', this.username);

    //    this.errorHandlingService.handleSuccess(`Welcome back, ${this.username}!`);
    //    // Redirect to dashboard
    //    this.router.navigate(['/dashboard']);
    //  } catch (error) {
    //    this.errorMessage = 'An error occurred during login';
    //    this.errorHandlingService.handleError(error, 'LOGIN_ERROR');
    //  } finally {
    //    this.isLoading = false;
    //  }
    //}, 1000);
  }

  resetForm(): void {
    // this.username = '';
    // this.password = '';
    // this.errorMessage = '';
  }
}
