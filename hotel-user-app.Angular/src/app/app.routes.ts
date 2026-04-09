import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';
import { RoomsComponent } from './features/rooms/rooms';
import { MyBookingsComponent } from './features/bookings/bookings';
import { authGuard, guestGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'rooms',
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [guestGuard],
    title: 'Login',
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [guestGuard],
    title: 'Register',
  },
  {
    path: 'rooms',
    component: RoomsComponent,
    canActivate: [authGuard],
    title: 'Rooms',
  },
  {
    path: 'my-bookings',
    component: MyBookingsComponent,
    canActivate: [authGuard],
    title: 'Bookings',
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
