import { Component, Inject } from '@angular/core';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/core/services/account/account.service';
import { ErrorHandlingService } from 'src/app/core/services/error-handling.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  protected isLoading = false;
  protected showPassword = false;
  
  protected registerForm = this.formBuilder.group({
    username: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  constructor(
    private router: Router,
    @Inject(ErrorHandlingService) private errorHandlingService: ErrorHandlingService,
    private formBuilder: UntypedFormBuilder,
    @Inject(AccountService) private accountService: AccountService,
  ) {}

  protected togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  protected onRegister(): void {
    this.isLoading = true;
    this.accountService.register(this.registerForm.value).subscribe({
      next: (response: any) => {
        this.isLoading = false;
        this.errorHandlingService.handleSuccess('Account created successfully!');
        this.router.navigate(['/dashboard']);
      },
      error: (error: any) => {
        this.isLoading = false;
        this.errorHandlingService.handleError(error, 'REGISTRATION_ERROR');
      }
    });
  }

  protected resetForm(): void {
  }

  protected goToLogin(): void {
    this.router.navigate(['/auth/login']);
  }
}
