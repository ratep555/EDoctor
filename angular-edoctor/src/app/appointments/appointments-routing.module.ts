import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentsListDoctorComponent } from './appointments-list-doctor/appointments-list-doctor.component';
import { AddAppointmentDoctorComponent } from './add-appointment-doctor/add-appointment-doctor.component';
import { EditAppointmentDoctorComponent } from './edit-appointment-doctor/edit-appointment-doctor.component';
import { AppointmentsComponent } from './appointments.component';
import { BookAppointmentPatientComponent } from './book-appointment-patient/book-appointment-patient.component';
import { ConfirmAppointmentDoctorComponent } from './confirm-appointment-doctor/confirm-appointment-doctor.component';
import { AppointmentsListPatientComponent } from './appointments-list-patient/appointments-list-patient.component';

const routes: Routes = [
  {path: '', component: AppointmentsComponent},
  {path: 'appointmentslistdoctor', component: AppointmentsListDoctorComponent},
  {path: 'appointmentslistpatient', component: AppointmentsListPatientComponent},
  {path: 'addappointmentdoctor', component: AddAppointmentDoctorComponent},
  {path: 'editappointmentdoctor/:id', component: EditAppointmentDoctorComponent},
  {path: 'bookappointmentpatient/:id', component: BookAppointmentPatientComponent},
  {path: 'confirmappointmentdoctor/:id', component: ConfirmAppointmentDoctorComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class AppointmentsRoutingModule { }
