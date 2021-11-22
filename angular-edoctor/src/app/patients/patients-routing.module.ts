import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddMedicalrecordComponent } from './add-medicalrecord/add-medicalrecord.component';
import { InfoPatientComponent } from './info-patient/info-patient.component';
import { InfoMedicalrecordComponent } from './info-medicalrecord/info-medicalrecord.component';
import { MedicalrecordsListDoctorComponent } from './medicalrecords-list-doctor/medicalrecords-list-doctor.component';
import { PatientsListDoctorComponent } from './patients-list-doctor/patients-list-doctor.component';
import { MedicalrecordsListPatientComponent } from './medicalrecords-list-patient/medicalrecords-list-patient.component';
import { InfoPatientMyprofileComponent } from './info-patient-myprofile/info-patient-myprofile.component';
import { EditPatientComponent } from './edit-patient/edit-patient.component';



const routes: Routes = [
  {path: 'patientslistdoctor', component: PatientsListDoctorComponent},
  {path: 'medicalrecordslistdoctor', component: MedicalrecordsListDoctorComponent},
  {path: 'medicalrecordslistpatient', component: MedicalrecordsListPatientComponent},
  {path: 'infopatientmyprofile', component: InfoPatientMyprofileComponent},
  {path: 'addmedicalrecord/:id', component: AddMedicalrecordComponent},
  {path: 'editpatient/:id', component: EditPatientComponent},
  {path: 'infopatient/:id', component: InfoPatientComponent},
  {path: 'infomedicalrecord/:id', component: InfoMedicalrecordComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class PatientsRoutingModule { }
