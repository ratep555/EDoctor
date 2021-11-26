import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditDoctorComponent } from './edit-doctor/edit-doctor.component';
import { SharedModule } from '../shared/shared.module';
import { DoctorsRoutingModule } from './doctors-routing.module';
import { InfoDoctorComponent } from './info-doctor/info-doctor.component';
import { InfoDoctorMyprofileComponent } from './info-doctor-myprofile/info-doctor-myprofile.component';
import { DoctorsComponent } from './doctors.component';
import { DoctorsListPatientComponent } from './doctors-list-patient/doctors-list-patient.component';
import { StatisticsDoctorComponent } from './statistics-doctor/statistics-doctor.component';
import { ChartPatientsDoctorComponent } from './charts-doctor/chart-patients-doctor/chart-patients-doctor.component';
import { ChartRecordsDoctorComponent } from './charts-doctor/chart-records-doctor/chart-records-doctor.component';


@NgModule({
  declarations: [
    EditDoctorComponent,
    InfoDoctorComponent,
    InfoDoctorMyprofileComponent,
    DoctorsComponent,
    DoctorsListPatientComponent,
    StatisticsDoctorComponent,
    ChartPatientsDoctorComponent,
    ChartRecordsDoctorComponent  ],
  imports: [
    CommonModule,
    SharedModule,
    DoctorsRoutingModule
  ]
})
export class DoctorsModule { }
