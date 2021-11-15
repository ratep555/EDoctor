import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddMedicalrecordComponent } from './add-medicalrecord/add-medicalrecord.component';
import { InfoPatientComponent } from './info-patient/info-patient.component';
import { SharedModule } from '../shared/shared.module';
import { PatientsRoutingModule } from './patients-routing.module';



@NgModule({
  declarations: [
    AddMedicalrecordComponent,
    InfoPatientComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PatientsRoutingModule
  ]
})
export class PatientsModule { }
