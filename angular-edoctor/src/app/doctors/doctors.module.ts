import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditDoctorComponent } from './edit-doctor/edit-doctor.component';
import { SharedModule } from '../shared/shared.module';
import { DoctorsRoutingModule } from './doctors-routing.module';
import { InfoDoctorComponent } from './info-doctor/info-doctor.component';
import { InfoDoctorMyprofileComponent } from './info-doctor-myprofile/info-doctor-myprofile.component';
import { DoctorsComponent } from './doctors.component';


@NgModule({
  declarations: [
    EditDoctorComponent,
    InfoDoctorComponent,
    InfoDoctorMyprofileComponent,
    DoctorsComponent  ],
  imports: [
    CommonModule,
    SharedModule,
    DoctorsRoutingModule
  ]
})
export class DoctorsModule { }
