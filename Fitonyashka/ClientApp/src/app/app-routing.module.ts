import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserLayoutComponent } from './layouts/user-layout/components/user-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/components/auth-layout.component';
import { GuestLayoutComponent } from './layouts/guest-layout/components/guest-layout.component';
import { AuthGuard } from './core/guards/auth.guard';
import { GuestGuard } from './core/guards/guest.guard';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  {
    path: 'home',
    component: GuestLayoutComponent,
    canActivate: [GuestGuard],
    children: [
      {
        path: '',
        loadChildren: () => import('./features/guest/guest.module').then(m => m.GuestModule),
      },
      {
        path: 'public',
        loadChildren: () => import('./features/public/public.module').then(m => m.PublicModule),
      },
    ],
  },
  {
    path: 'auth',
    component: AuthLayoutComponent,
    canActivate: [GuestGuard],
    children: [
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
      {
        path: 'public',
        loadChildren: () => import('./features/public/public.module').then(m => m.PublicModule),
      },
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
