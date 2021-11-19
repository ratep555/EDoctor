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



@NgModule({
  declarations: [
    AddMedicalrecordComponent,
    InfoPatientComponent,
    InfoMedicalrecordComponent,
    MedicalrecordsListDoctorComponent,
    PatientsListDoctorComponent,
    MedicalrecordsListPatientComponent,
    InfoPatientMyprofileComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PatientsRoutingModule
  ]
})
export class PatientsModule { }
