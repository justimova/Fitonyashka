import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/core/services/account/account.service';
import { BmiService } from 'src/app/core/services/bmi/bmi.service';
import { UserProfileService } from 'src/app/core/services/account/user-profile.service';
import { ICalculatedBmi, IBmiRange } from 'src/app/core/models/bmi/bmi';

@Component({
  selector: 'app-bmi',
  templateUrl: './bmi.component.html',
  styleUrls: ['./bmi.component.scss']
})
export class BmiComponent implements OnInit {
  protected isLoggedIn = false;
  protected bmiForm: FormGroup;
  protected weightCalculatorForm: FormGroup;
  protected calculatedBmi: ICalculatedBmi | null = null;
  protected calculatedWeight: number | null = null;
  protected bmiRanges: IBmiRange[] = [];
  protected currentUserData: { weight: number; height: number; bmi: ICalculatedBmi | null } = { weight: 0, height: 0, bmi: null };

  constructor(
    private accountService: AccountService,
    private bmiService: BmiService,
    private userProfileService: UserProfileService,
    private fb: FormBuilder,
  ) {
    this.bmiForm = this.fb.group({
      weight: [0, [Validators.required, Validators.min(0.1)]],
      height: [0, [Validators.required, Validators.min(0.1)]],
    });

    this.weightCalculatorForm = this.fb.group({
      height: [0, [Validators.required, Validators.min(0.1)]],
      bmiValue: [0, [Validators.required, Validators.min(0.1), Validators.max(100)]],
    });
  }

  public ngOnInit(): void {
    this.isLoggedIn = this.accountService.isLoggedIn();
    this.bmiService.getCategory().subscribe(ranges => {
      this.bmiRanges = ranges;

      if (this.isLoggedIn) {
        this.userProfileService.getCurrentUser().subscribe(user => {
          this.currentUserData.weight = user.weight;
          this.currentUserData.height = user.height;
          this.weightCalculatorForm.patchValue({ height: user.height });
          this.bmiForm.patchValue({ height: user.height });
          this.bmiService.calculate(this.currentUserData.height, this.currentUserData.weight)
            .subscribe(result => {
              this.currentUserData.bmi = result;
            });
        });
      }
    });
  }

  protected onCalculate(): void {
    if (this.bmiForm.invalid) {
      this.bmiForm.markAllAsTouched();
      return;
    }

    const weight = Number(this.bmiForm.get('weight')?.value) || 0;
    const height = Number(this.bmiForm.get('height')?.value) || 0;

    this.bmiService.calculate(height, weight)
      .subscribe(result => {
        this.calculatedBmi = result;
      });
  }

  protected onCalculateWeight(): void {
    if (this.weightCalculatorForm.invalid) {
      this.weightCalculatorForm.markAllAsTouched();
      return;
    }

    const height = Number(this.weightCalculatorForm.get('height')?.value) || 0;
    const bmiValue = Number(this.weightCalculatorForm.get('bmiValue')?.value) || 0;

    this.bmiService.calculateWeight(height, bmiValue)
      .subscribe(result => {
        this.calculatedWeight = typeof result === 'number'
          ? result
          : result?.weight ?? null;
      });
  }

  protected checkError(controlName: string, errorName: string, formName: 'bmi' | 'weight' = 'bmi'): boolean {
    const formGroup = formName === 'bmi' ? this.bmiForm : this.weightCalculatorForm;
    const control = formGroup.get(controlName);
    return control ? control.hasError(errorName) && control.touched : false;
  }

  protected getRangeDisplay(range: IBmiRange): string {
    if (range.min === 0) {
      return `&lt; ${range.max}`;
    } else if (range.max === Infinity || range.max > 999) { // assuming Infinity or large number for obesity
      return `≥ ${range.min}`;
    } else {
      return `${range.min} - ${range.max}`;
    }
  }
}
