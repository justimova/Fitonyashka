import { Component, OnInit, OnDestroy, DestroyRef, inject, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { WeightService } from '../../../../core/services/weight/weight.service';
import { GoalService } from '../../../../core/services/goal/goal.service';
import { IWeightCreate, IWeightUpdate } from '../../../../core/models/weight/weight';
import { ErrorHandlingService } from 'src/app/core/services/error-handling.service';
import { NotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-weight-form',
  templateUrl: './weight-form.component.html',
  styleUrls: ['./weight-form.component.scss']
})
export class WeightFormComponent implements OnInit {
  protected isSaving = false;
  protected isLoading = false;
  protected title = 'Add Weight Entry';

  private destroyRef = inject(DestroyRef);
  private weightId: number | null = null;

  protected weightForm = this.formBuilder.group({
    date: [new Date().toISOString().split('T')[0], [Validators.required]],
    weight: ['', [Validators.required, Validators.min(0), Validators.max(500)]],
  });

  constructor(
    private dialogRef: MatDialogRef<WeightFormComponent>,
    private formBuilder: FormBuilder,
    private weightService: WeightService,
    private goalService: GoalService,
    private errorHandlingService: ErrorHandlingService,
    private notificationService: NotificationService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) private dialogData: any,
  ) {
    this.weightId = this.dialogData?.id || null;
  }

  public ngOnInit(): void {
    if (this.weightId) {
      this.title = 'Edit Weight Entry';
      this._loadWeightInfo();
    }
  }

  protected checkError(controlName: string, errorName: string): boolean {
    const control = this.weightForm.get(controlName);
    return control ? control.hasError(errorName) && control.touched : false;
  }

  protected onClose(): void {
    this.dialogRef.close();
  }

  protected onSave(): void {
    if (this.weightForm.invalid) {
      this.weightForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    if (this.weightId) {
      this._handleUpdate();
    } else {
      this._handleCreate();
    }
  }

  private _handleCreate(): void {
    const model: IWeightCreate = {
      date: this.weightForm.controls['date'].value as string,
      weight: Number(this.weightForm.controls['weight'].value),
    };

    this.weightService.create(model)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (response) => {
          this.isSaving = false;
          this.errorHandlingService.handleResponse(response);
          if (response.isSuccess) {
            this._checkGoalAchievement();
          }
        },
        error: (error) => {
          this.isSaving = false;
          this.errorHandlingService.handleError(error, 'WEIGHT_CREATE_ERROR');
        }
      });
  }

  private _handleUpdate(): void {
    const model: IWeightUpdate = {
      id: this.weightId!,
      date: this.weightForm.controls['date'].value as string,
      weight: Number(this.weightForm.controls['weight'].value),
    };

    this.weightService.update(model)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (result) => {
          this.isSaving = false;
          this.errorHandlingService.handleResponse(result);
          if (result.isSuccess) {
            this._checkGoalAchievement();
          }
        },
        error: (error) => {
          this.isSaving = false;
          this.errorHandlingService.handleError(error, 'WEIGHT_EDIT_ERROR');
        }
      });
  }

  private _loadWeightInfo(): void {
    if (!this.weightId) {
      return;
    }

    this.isLoading = true;
    this.weightService.getInfo(this.weightId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (weight) => {
          this.weightForm.patchValue({
            date: weight.date,
            weight: weight.weight.toString(),
          });
          this.isLoading = false;
        },
        error: (error) => {
          this.isLoading = false;
          this.errorHandlingService.handleError(error, 'WEIGHT_LOAD_ERROR');
        }
      });
  }

  private _checkGoalAchievement(): void {
    this.goalService.completeGoalIfNeeded()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (goalCompleted) => {
          if (goalCompleted) {
            this.notificationService.showSuccessWithLink(
              'Congratulations! You have achieved your goal!',
              'Set a New Goal',
              '/goals'
            );
            setTimeout(() => {
              this.dialogRef.close(true);
            }, 2000);
          } else {
            this.dialogRef.close(true);
          }
        },
        error: (error) => {
          console.error('Error checking goal achievement:', error);
          this.dialogRef.close(true);
        }
      });
  }
}
