import { Routes } from '@angular/router';
import { AuthLayout } from './layouts/auth-layout/auth-layout';
import { MainLayout } from './layouts/main-layout/main-layout';
import { Login } from './features/auth/login/login';
import { UserDashboard } from './features/users/user-dashboard/user-dashboard';
import { Home } from './features/home/home';
import { Register } from './features/auth/register/register';
import { ServerError } from './shared/components/server-error/server-error';

export const routes: Routes = [
  {
    path: '',
    component: MainLayout,
    children: [
      { path: '', component: Home },
      { path: 'dashboard', component: UserDashboard },
    ],
  },
  {
    path: '',
    component: AuthLayout,
    children: [
      { path: 'login', component: Login },
      { path: 'register', component: Register },
    ],
  },
  {
    path: 'server-error',
    component: ServerError,
  },
];
