import { Component, OnInit, OnDestroy, DestroyRef, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { WeightService } from '../../../../core/services/weight/weight.service';
import { WeightFormComponent } from './weight-form.component';
import { IWeight } from 'src/app/core/models/weight/weight';

@Component({
  selector: 'app-weight',
  standalone: false,
  templateUrl: './weight.component.html',
  styleUrl: './weight.component.scss'
})
export class WeightComponent implements OnInit {
  protected weightHistory: IWeight[] = [];
  protected isLoading = false;
  
  private destroyRef = inject(DestroyRef);

  constructor(
    private weightService: WeightService,
    private _matDialog: MatDialog,
  ) { }

  public ngOnInit(): void {
    this._loadData();
  }

  public onOpenForm(id: number | null): void {
    const dialogConfig = {
      width: '672px',
      autoFocus: false,
      disableClose: true,
      data: {
        id: id,
      },
    };

    this._matDialog
      .open(WeightFormComponent, dialogConfig)
      .afterClosed()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(response => {
        if (response) {
          this._loadData();
        }
      });
  }

  public onDelete(id: number): void {
    const confirmed = window.confirm('Are you sure you want to delete this weight entry?');
    if (!confirmed) {
      return;
    }

    this.weightService.delete(id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (result) => {
          if (result.isSuccess) {
            this._loadData();
          } else {
            console.error('Delete weight failed:', result.errorMessage);
          }
        },
        error: (error) => {
          console.error('Failed to delete weight:', error);
        }
      });
  }

  private _loadData(): void {
    this.isLoading = true;
    this.weightService.getWeights()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (weights) => {
          this.weightHistory = weights;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Failed to load weights:', error);
          this.isLoading = false;
        }
      });
  }
}

// TODO: сделать хэдер компонент не стендэлон. посмотреть коммит к задаче #63 