import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InfoDoctorComponent } from './info-doctor/info-doctor.component';
import { EditDoctorComponent } from './edit-doctor/edit-doctor.component';
import { InfoDoctorMyprofileComponent } from './info-doctor-myprofile/info-doctor-myprofile.component';
import { DoctorsComponent } from './doctors.component';
import { DoctorsListPatientComponent } from './doctors-list-patient/doctors-list-patient.component';
import { StatisticsDoctorComponent } from './statistics-doctor/statistics-doctor.component';


const routes: Routes = [
  {path: '', component: DoctorsComponent},
  {path: 'doctorslistpatient', component: DoctorsListPatientComponent},
  {path: 'infodoctormyprofile', component: InfoDoctorMyprofileComponent},
  {path: 'statisticsdoctor', component: StatisticsDoctorComponent},
  {path: 'editdoctor/:id', component: EditDoctorComponent},
  {path: 'infodoctor/:id', component: InfoDoctorComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class DoctorsRoutingModule { }
