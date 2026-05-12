import { Component, Input } from '@angular/core';
import { ChartConfiguration } from 'chart.js';
import { IWeight } from 'src/app/core/models/weight/weight';

@Component({
  selector: 'app-weight-chart',
  standalone: false,
  templateUrl: './weight-chart.component.html',
  styleUrl: './weight-chart.component.scss'
})
export class WeightChartComponent {
  @Input() public entries: IWeight[] = [];
  
  protected get chartData(): ChartConfiguration<'line'>['data'] {
    return {
      labels: this.entries.map(x => x.date),
      datasets: [
        {
          label: 'Weight',
          data: this.entries.map(x => x.weight)
        }
      ]
    };
  }

  protected get chartOptions(): ChartConfiguration<'line'>['options'] {
    return {
      plugins: {
        legend: {
          display: false
        }
      }
    };
  }
}
