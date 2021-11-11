import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddOfficeDoctorComponent } from './add-office-doctor/add-office-doctor.component';
import { SharedModule } from '../shared/shared.module';
import { OfficesRoutingModule } from './offices-routing.module';
import { OfficesListDoctorComponent } from './offices-list-doctor/offices-list-doctor.component';
import { EditOfficeDoctorComponent } from './edit-office-doctor/edit-office-doctor.component';
import { OfficesComponent } from './offices.component';



@NgModule({
  declarations: [
    AddOfficeDoctorComponent,
    OfficesListDoctorComponent,
    EditOfficeDoctorComponent,
    OfficesComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    OfficesRoutingModule
  ]
})
export class OfficesModule { }
