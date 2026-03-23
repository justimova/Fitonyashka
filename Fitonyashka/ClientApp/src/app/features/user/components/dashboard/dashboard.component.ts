import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  stats = [
    { title: 'Total Workouts', value: 24, icon: 'fa-dumbbell', color: '#667eea' },
    { title: 'Calories Burned', value: '3,240', icon: 'fa-fire', color: '#764ba2' },
    { title: 'Current Streak', value: '12 days', icon: 'fa-flame', color: '#f39c12' },
    { title: 'Goals Achieved', value: 5, icon: 'fa-trophy', color: '#27ae60' }
  ];

  recentActivities = [
    { name: 'Morning Run', date: new Date(Date.now() - 1 * 24 * 60 * 60 * 1000), duration: '45 min' },
    { name: 'Weight Training', date: new Date(Date.now() - 2 * 24 * 60 * 60 * 1000), duration: '60 min' },
    { name: 'Yoga Session', date: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000), duration: '30 min' }
  ];
}
