import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { IsAdminGuard } from './core/guards/is-admin.guard';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { HomeComponent } from './home/home/home.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'appointments', canActivate: [AuthGuard],
  loadChildren: () => import('./appointments/appointments.module').then(mod => mod.AppointmentsModule)},
  {path: 'offices', canActivate: [AuthGuard],
  loadChildren: () => import('./offices/offices.module').then(mod => mod.OfficesModule)},
  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule)},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
