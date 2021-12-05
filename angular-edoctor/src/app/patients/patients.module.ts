import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddMedicalrecordComponent } from './add-medicalrecord/add-medicalrecord.component';
import { InfoPatientComponent } from './info-patient/info-patient.component';
import { SharedModule } from '../shared/shared.module';
import { PatientsRoutingModule } from './patients-routing.module';
import { InfoMedicalrecordComponent } from './info-medicalrecord/info-medicalrecord.component';
import { MedicalrecordsListDoctorComponent } from './medicalrecords-list-doctor/medicalrecords-list-doctor.component';
import { PatientsListDoctorComponent } from './patients-list-doctor/patients-list-doctor.component';
import { MedicalrecordsListPatientComponent } from './medicalrecords-list-patient/medicalrecords-list-patient.component';
import { InfoPatientMyprofileComponent } from './info-patient-myprofile/info-patient-myprofile.component';
import { EditPatientComponent } from './edit-patient/edit-patient.component';
import { StatisticsPatientComponent } from './statistics-patient/statistics-patient.component';
import { ChartOfficesPatientComponent } from './charts-patient/chart-offices-patient/chart-offices-patient.component';
import { ChartAppointmentsPatientComponent } from './charts-patient/chart-appointments-patient/chart-appointments-patient.component';
import { EditMedicalrecordComponent } from './edit-medicalrecord/edit-medicalrecord.component';



@NgModule({
  declarations: [
    AddMedicalrecordComponent,
    InfoPatientComponent,
    InfoMedicalrecordComponent,
    MedicalrecordsListDoctorComponent,
    PatientsListDoctorComponent,
    MedicalrecordsListPatientComponent,
    InfoPatientMyprofileComponent,
    EditPatientComponent,
    StatisticsPatientComponent,
    ChartOfficesPatientComponent,
    ChartAppointmentsPatientComponent,
    EditMedicalrecordComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PatientsRoutingModule
  ]
})
export class PatientsModule { }
