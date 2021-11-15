import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddAppointmentDoctorComponent } from './add-appointment-doctor/add-appointment-doctor.component';
import { EditAppointmentDoctorComponent } from './edit-appointment-doctor/edit-appointment-doctor.component';
import { AppointmentsListDoctorComponent } from './appointments-list-doctor/appointments-list-doctor.component';
import { SharedModule } from '../shared/shared.module';
import { AppointmentsRoutingModule } from './appointments-routing.module';
import { AppointmentsListPatientComponent } from './appointments-list-patient/appointments-list-patient.component';
import { AppointmentsComponent } from './appointments.component';
import { BookAppointmentPatientComponent } from './book-appointment-patient/book-appointment-patient.component';
import { ConfirmAppointmentDoctorComponent } from './confirm-appointment-doctor/confirm-appointment-doctor.component';



@NgModule({
  declarations: [
    AddAppointmentDoctorComponent,
    EditAppointmentDoctorComponent,
    AppointmentsListDoctorComponent,
    AppointmentsListPatientComponent,
    AppointmentsComponent,
    BookAppointmentPatientComponent,
    ConfirmAppointmentDoctorComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AppointmentsRoutingModule
  ]
})
export class AppointmentsModule { }
