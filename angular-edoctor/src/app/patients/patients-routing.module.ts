import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AddMedicalrecordComponent } from './add-medicalrecord/add-medicalrecord.component';
import { InfoPatientComponent } from './info-patient/info-patient.component';



const routes: Routes = [
  {path: 'addmedicalrecord', component: AddMedicalrecordComponent},
  {path: 'infopatient/:id', component: InfoPatientComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class PatientsRoutingModule { }
