import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/core/services/account/account.service';
import { BmiService } from 'src/app/core/services/bmi/bmi.service';
import { ICalculatedBmi, IBmiRange } from 'src/app/core/models/bmi/bmi';

@Component({
  selector: 'app-bmi',
  templateUrl: './bmi.component.html',
  styleUrls: ['./bmi.component.scss']
})
export class BmiComponent implements OnInit {
  protected isLoggedIn = false;
  protected bmiForm: FormGroup;
  protected calculatedBmi: ICalculatedBmi | null = null;
  protected bmiRanges: IBmiRange[] = [];

  constructor(
    private accountService: AccountService,
    private bmiService: BmiService,
    private fb: FormBuilder,
  ) {
    this.bmiForm = this.fb.group({
      weight: [0, [Validators.min(0)]],
      height: [0, [Validators.min(0)]],
    });
  }

  public ngOnInit(): void {
    this.isLoggedIn = this.accountService.isLoggedIn();
    this.bmiService.getCategory().subscribe(ranges => {
      this.bmiRanges = ranges;
    });
  }

  protected onCalculate(): void {
    const weight = Number(this.bmiForm.get('weight')?.value) || 0;
    const height = Number(this.bmiForm.get('height')?.value) || 0;

    this.bmiService.calculate(height, weight)
      .subscribe(result => {
        this.calculatedBmi = result;
      });
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

// на бэке сделать bmi controller создать bmi service и запрограмировать форму 