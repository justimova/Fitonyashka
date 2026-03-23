import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserLayoutComponent } from './layouts/user-layout/components/user-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/components/auth-layout.component';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full',
      },
      {
        path: '',
        loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule),
      },
    ],
  },

  {
    path: '',
    component: UserLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
      {
        path: '',
        loadChildren: () => import('./features/user/user.module').then(m => m.UserModule),
      },
    ],
  },

  // Wildcard route - must be last
  {
    path: '**',
    redirectTo: '/dashboard',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
