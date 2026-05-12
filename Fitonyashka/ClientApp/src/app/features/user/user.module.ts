import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { UserRoutingModule } from './user-routing.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { WeightComponent } from './components/weight/weight.component';
import { WeightChartComponent } from './components/weight/weight-chart.component';
import { MatDialogModule } from '@angular/material/dialog';
import { WeightFormComponent } from './components/weight/weight-form.component';
import { BaseChartDirective } from 'ng2-charts';

@NgModule({
  declarations: [
    DashboardComponent,
    WeightComponent,
    WeightFormComponent,
    WeightChartComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    UserRoutingModule,
    ReactiveFormsModule,
    MatDialogModule,
    BaseChartDirective,
  ]
})
export class UserModule { }
