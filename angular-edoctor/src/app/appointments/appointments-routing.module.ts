import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentsListDoctorComponent } from './appointments-list-doctor/appointments-list-doctor.component';
import { AddAppointmentDoctorComponent } from './add-appointment-doctor/add-appointment-doctor.component';
import { EditAppointmentDoctorComponent } from './edit-appointment-doctor/edit-appointment-doctor.component';
import { AppointmentsComponent } from './appointments.component';

const routes: Routes = [
  {path: '', component: AppointmentsComponent},
  {path: 'appointmentslistdoctor', component: AppointmentsListDoctorComponent},
  {path: 'addappointmentdoctor', component: AddAppointmentDoctorComponent},
  {path: 'editappointmentdoctor/:id', component: EditAppointmentDoctorComponent},

];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class AppointmentsRoutingModule { }
