import { Component, OnInit, inject, DestroyRef, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { GoalService } from 'src/app/core/services/goal/goal.service';
import { IGoalCreate } from 'src/app/core/models/goal/goal';
import { ErrorHandlingService } from 'src/app/core/services/error-handling.service';

@Component({
  selector: 'app-goal-form',
  templateUrl: './goal-form.component.html',
  styleUrls: ['./goal-form.component.scss']
})
export class GoalFormComponent implements OnInit {
  protected title = 'Set Goal';
  protected isSaving = false;
  protected isLoading = false;

  private destroyRef = inject(DestroyRef);

  protected goalForm = this.formBuilder.group({
    currentWeight: [{ value: 0, disabled: true }, [Validators.required]],
    targetWeight: ['', [Validators.required, Validators.min(0), Validators.max(500)]],
  });

  constructor(
    private dialogRef: MatDialogRef<GoalFormComponent>,
    private formBuilder: FormBuilder,
    private goalService: GoalService,
    private errorHandlingService: ErrorHandlingService,
    @Inject(MAT_DIALOG_DATA) private dialogData: { currentWeight: number },
  ) {}

  public ngOnInit(): void {
    if (this.dialogData?.currentWeight != null) {
      this.goalForm.patchValue({ currentWeight: this.dialogData.currentWeight });
    }
  }

  protected checkError(controlName: string, errorName: string): boolean {
    const control = this.goalForm.get(controlName);
    return control ? control.hasError(errorName) && control.touched : false;
  }

  protected onClose(): void {
    this.dialogRef.close();
  }

  protected onSave(): void {
    if (this.goalForm.invalid) {
      this.goalForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;

    const model: IGoalCreate = {
      initialWeight: Number(this.goalForm.get('currentWeight')?.value),
      targetWeight: Number(this.goalForm.get('targetWeight')?.value),
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
