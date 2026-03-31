import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './components/home/home.component';
import { GuestRoutingModule } from './guest-routing.module';

@NgModule({
  declarations: [
    HomeComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    GuestRoutingModule,
    ReactiveFormsModule,
  ]
})
export class GuestModule { }
