import { Component, OnInit, inject, DestroyRef, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { GoalService } from 'src/app/core/services/goal/goal.service';
import { IGoalCreate, IGoalUpdate } from 'src/app/core/models/goal/goal';
import { ErrorHandlingService } from 'src/app/core/services/error-handling.service';
import { UserProfileService } from 'src/app/core/services/account/user-profile.service';

@Component({
  selector: 'app-goal-form',
  templateUrl: './goal-form.component.html',
  styleUrls: ['./goal-form.component.scss']
})
export class GoalFormComponent implements OnInit {
  protected isSaving = false;
  protected isLoading = false;
  protected currentWeight = 0;
  protected weightChanged = 0;
  protected weightRemaining = 0;
  protected isGainingWeight = false;
  protected isCurrentWeightSet = false;
  protected currentGoalInfo: any = null;

  private destroyRef = inject(DestroyRef);
  private initialWeight = 0;
  private _goalId: number | null = null;

  get goalId(): number | null {
    return this._goalId;
  }

  protected goalForm = this.formBuilder.group({
    currentWeight: [{ value: 0, disabled: true }, [Validators.required, Validators.min(0.1)]],
    targetWeight: [0, [Validators.required, Validators.min(0.1), Validators.max(500)]],
  });

  constructor(
    private dialogRef: MatDialogRef<GoalFormComponent>,
    private formBuilder: FormBuilder,
    private goalService: GoalService,
    private errorHandlingService: ErrorHandlingService,
    private userProfileService: UserProfileService,
    @Inject(MAT_DIALOG_DATA) private dialogData: { goalId: number | null; currentWeight: number },
  ) {}

  public ngOnInit(): void {
    this._goalId = this.dialogData?.goalId ?? null;
    
    if (this._goalId !== null) {
      this._loadGoalInfo();
    } else {
      this._loadCurrentWeight();
    }
  }

  private _loadCurrentWeight(): void {
    this.userProfileService.getCurrentUser()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (user) => {
          if (user.weight > 0) {
            this.currentWeight = user.weight;
            this.initialWeight = user.weight;
            this.isCurrentWeightSet = true;
            this.goalForm.get('currentWeight')?.disable();
          } else {
            this.isCurrentWeightSet = false;
            this.goalForm.get('currentWeight')?.enable();
          }
          this.goalForm.patchValue({ currentWeight: user.weight });
          this.updateWeightInfo();
        },
        error: (error) => {
          console.error('Failed to load current weight:', error);
          this.isCurrentWeightSet = false;
          this.goalForm.get('currentWeight')?.enable();
        }
      });
  }

  private _loadGoalInfo(): void {
    if (this._goalId === null) {
      return;
    }

    this.isLoading = true;
    this.goalService.getGoalById(this._goalId)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (goalInfo) => {
          this.currentGoalInfo = goalInfo;
          this.initialWeight = goalInfo.initialWeight;
          this.isCurrentWeightSet = true;
          this.goalForm.get('currentWeight')?.disable();
          this.goalForm.patchValue({
            currentWeight: goalInfo.initialWeight,
            targetWeight: goalInfo.targetWeight,
          });
          this.isLoading = false;
          // Get current weight from user profile
          this.userProfileService.getCurrentUser()
            .pipe(takeUntilDestroyed(this.destroyRef))
            .subscribe({
              next: (user) => {
                this.currentWeight = user.weight;
                this.updateWeightInfo();
              },
              error: (error) => {
                console.error('Failed to load current weight:', error);
              }
            });
        },
        error: (error) => {
          console.error('Failed to load goal info:', error);
          this.isLoading = false;
        }
      });
  }

  private updateWeightInfo(): void {
    const targetWeight = Number(this.goalForm.get('targetWeight')?.value) || 0;
    
    // Determine if we're gaining or losing weight
    this.isGainingWeight = this.initialWeight < targetWeight;
    
    // Calculate change from initial weight
    this.weightChanged = Math.abs(this.currentWeight - this.initialWeight);
    
    // Calculate remaining to reach target
    this.weightRemaining = Math.abs(targetWeight - this.currentWeight);
  }

  protected checkError(controlName: string, errorName: string): boolean {
    const control = this.goalForm.get(controlName);
    return control ? control.hasError(errorName) && control.touched : false;
  }

  protected onClose(): void {
    this.dialogRef.close();
  }

  protected onReset(): void {
    if (this.currentGoalInfo) {
      if (confirm('Are you sure you want to reset your goal?')) {
        this.isLoading = true;
        this.goalService.deleteGoal(this.currentGoalInfo.id)
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe({
            next: (response) => {
              this.isLoading = false;
              this.errorHandlingService.handleResponse(response);
              if (response.isSuccess) {
                this.dialogRef.close(true);
              }
            },
            error: (error) => {
              this.isLoading = false;
              this.errorHandlingService.handleError(error, 'GOAL_RESET_ERROR');
            }
          });
      }
    } else {
      this.goalForm.patchValue({ targetWeight: 0 });
      this.updateWeightInfo();
    }
  }

  protected onSave(): void {
    if (this.goalForm.invalid) {
      this.goalForm.markAllAsTouched();
      return;
    }

    const initialWeight = Number(this.goalForm.get('currentWeight')?.value);
    const targetWeight = Number(this.goalForm.get('targetWeight')?.value);
    const targetControl = this.goalForm.get('targetWeight');

    if (Math.abs(targetWeight - initialWeight) < 0.0001) {
      const currentErrors = targetControl?.errors ?? {};
      targetControl?.setErrors({
        ...currentErrors,
        sameAsInitial: true,
      });
      targetControl?.markAsTouched();
      return;
    }

    this.isSaving = true;
    this.updateWeightInfo();

    if (this._goalId !== null) {
      const model: IGoalUpdate = {
        id: this._goalId,
        initialWeight,
        targetWeight,
      };

      this.goalService.updateGoal(model)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (response) => {
            this.isSaving = false;
            this.errorHandlingService.handleResponse(response);
            if (response.isSuccess) {
              this.dialogRef.close(true);
            }
          },
          error: (error) => {
            this.isSaving = false;
            const errorKey = this.isGainingWeight ? 'GOAL_UPDATE_ERROR' : 'GOAL_UPDATE_ERROR';
            this.errorHandlingService.handleError(error, errorKey);
          }
        });
    } else {
      const model: IGoalCreate = {
        initialWeight,
        targetWeight,
      };

      this.goalService.setGoal(model)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (response) => {
            this.isSaving = false;
            this.errorHandlingService.handleResponse(response);
            if (response.isSuccess) {
              this.dialogRef.close(true);
            }
          },
          error: (error) => {
            this.isSaving = false;
            this.errorHandlingService.handleError(error, 'GOAL_SET_ERROR');
          }
        });
    }
  }
}
