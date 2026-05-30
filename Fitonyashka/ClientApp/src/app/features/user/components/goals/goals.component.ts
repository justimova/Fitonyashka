import { Component, OnInit, DestroyRef, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UserProfileService } from 'src/app/core/services/account/user-profile.service';
import { GoalService } from 'src/app/core/services/goal/goal.service';
import { IGoalInfo } from 'src/app/core/models/goal/goal';
import { GoalFormComponent } from './goal-form.component';
import { ErrorHandlingService } from 'src/app/core/services/error-handling.service';

@Component({
  selector: 'app-goals',
  standalone: false,
  templateUrl: './goals.component.html',
  styleUrls: ['./goals.component.scss']
})
export class GoalsComponent implements OnInit {
  protected currentWeight: number | null = null;
  protected currentGoal: IGoalInfo | null = null;
  protected isLoading = false;
  protected expandedGoalType: string | null = null;

  private destroyRef = inject(DestroyRef);

  constructor(
    private _matDialog: MatDialog,
    private userProfileService: UserProfileService,
    private goalService: GoalService,
    private errorHandlingService: ErrorHandlingService,
  ) { }

  public ngOnInit(): void {
    this._loadCurrentWeight();
    this._loadCurrentGoal();
  }

  public onSetGoal(): void {
    if (this.currentWeight === null) {
      return;
    }

    const dialogConfig = {
      width: '672px',
      autoFocus: false,
      disableClose: true,
      data: {
        currentWeight: this.currentWeight,
      },
    };

    this._matDialog
      .open(GoalFormComponent, dialogConfig)
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(response => {
        if (response) {
          this._loadCurrentWeight();
          this._loadCurrentGoal();
        }
      });
  }

  public onResetGoal(): void {
    if (!this.currentGoal) {
      return;
    }

    if (confirm('Are you sure you want to reset your goal?')) {
      this.isLoading = true;
      this.goalService.deleteGoal(this.currentGoal.id)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe({
          next: (response) => {
            this.currentGoal = null;
            this.isLoading = false;
            this.errorHandlingService.handleResponse(response);
          },
          error: (error) => {
            this.isLoading = false;
            this.errorHandlingService.handleError(error, 'GOAL_RESET_ERROR');
          }
        });
    }
  }

  public onTileExpand(goalType: string): void {
    if (this.expandedGoalType === goalType) {
      this.expandedGoalType = null;
    } else {
      this.expandedGoalType = goalType;
    }
  }

  public onTileClose(): void {
    this.expandedGoalType = null;
  }

  private _loadCurrentWeight(): void {
    this.isLoading = true;
    this.userProfileService.getCurrentUser()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (user) => {
          this.currentWeight = user.weight;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Failed to load current weight:', error);
          this.isLoading = false;
        }
      });
  }

  private _loadCurrentGoal(): void {
    this.goalService.getCurrentGoal()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (goal) => {
          this.currentGoal = goal;
        },
        error: (error) => {
          console.error('Failed to load current goal:', error);
          this.currentGoal = null;
        }
      });
  }
}

// бэкенд установка целей, после установки цели как отображать на странице, кнопка цель достигнута, кнопка удалить
// 1. если веса в профиле нет то в окне указать input для ввода веса на текушую дату если вес в профиле есть то этот инпут disable
// 3. если цель достигнута то на странице веса при добавлении новой цели писать поздравлялку и предлагать установить новую цель, если цель не достигнута то на странице веса показывать сколько осталось до цели
// сделать норм отображение
// реализовать эдит цели.