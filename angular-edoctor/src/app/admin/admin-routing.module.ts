import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { UsersListComponent } from './users-list/users-list.component';
import { RegisterDoctorComponent } from './register-doctor/register-doctor.component';
import { HospitalsListAdminComponent } from './hospitals/hospitals-list-admin/hospitals-list-admin.component';
import { AddHospitalAdminComponent } from './hospitals/add-hospital-admin/add-hospital-admin.component';
import { EditHospitalAdminComponent } from './hospitals/edit-hospital-admin/edit-hospital-admin.component';
import { SpecialtiesListAdminComponent } from './specialties/specialties-list-admin/specialties-list-admin.component';
import { AddSpecialtyAdminComponent } from './specialties/add-specialty-admin/add-specialty-admin.component';
import { EditSpecialtyAdminComponent } from './specialties/edit-specialty-admin/edit-specialty-admin.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { GendersListAdminComponent } from './genders/genders-list-admin/genders-list-admin.component';
import { AddGenderAdminComponent } from './genders/add-gender-admin/add-gender-admin.component';
import { EditGenderAdminComponent } from './genders/edit-gender-admin/edit-gender-admin.component';


const routes: Routes = [
  {path: 'userslist', component: UsersListComponent},
  {path: 'register-doctor', component: RegisterDoctorComponent},
  {path: 'statistics', component: StatisticsComponent},
  {path: 'genderslistadmin', component: GendersListAdminComponent},
  {path: 'hospitalslistadmin', component: HospitalsListAdminComponent},
  {path: 'specialtieslistadmin', component: SpecialtiesListAdminComponent},
  {path: 'addgenderadmin', component: AddGenderAdminComponent},
  {path: 'editgenderadmin/:id', component: EditGenderAdminComponent},
  {path: 'addhospitaladmin', component: AddHospitalAdminComponent},
  {path: 'edithospitaladmin/:id', component: EditHospitalAdminComponent},
  {path: 'addspecialtyadmin', component: AddSpecialtyAdminComponent},
  {path: 'editspecialtyadmin/:id', component: EditSpecialtyAdminComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
