import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AddOfficeDoctorComponent } from './add-office-doctor/add-office-doctor.component';
import { OfficesListDoctorComponent } from './offices-list-doctor/offices-list-doctor.component';
import { EditOfficeDoctorComponent } from './edit-office-doctor/edit-office-doctor.component';
import { OfficesComponent } from './offices.component';
import { InfoOfficeComponent } from './info-office/info-office.component';

const routes: Routes = [
  {path: '', component: OfficesComponent},
  {path: 'officeslistdoctor', component: OfficesListDoctorComponent},
  {path: 'addofficedoctor', component: AddOfficeDoctorComponent},
  {path: 'editofficedoctor/:id', component: EditOfficeDoctorComponent},
  {path: 'infooffice/:id', component: InfoOfficeComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OfficesRoutingModule { }
