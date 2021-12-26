import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersListComponent } from './users-list/users-list.component';
import { SharedModule } from '../shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { RegisterDoctorComponent } from './register-doctor/register-doctor.component';
import { HospitalsListAdminComponent } from './hospitals/hospitals-list-admin/hospitals-list-admin.component';
import { AddHospitalAdminComponent } from './hospitals/add-hospital-admin/add-hospital-admin.component';
import { EditHospitalAdminComponent } from './hospitals/edit-hospital-admin/edit-hospital-admin.component';
import { SpecialtiesListAdminComponent } from './specialties/specialties-list-admin/specialties-list-admin.component';
import { AddSpecialtyAdminComponent } from './specialties/add-specialty-admin/add-specialty-admin.component';
import { EditSpecialtyAdminComponent } from './specialties/edit-specialty-admin/edit-specialty-admin.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { ChartOfficesComponent } from './charts/chart-offices/chart-offices.component';
import { ChartAppointmentsComponent } from './charts/chart-appointments/chart-appointments.component';
import { ChartPatientsComponent } from './charts/chart-patients/chart-patients.component';
import { ChartGendersComponent } from './charts/chart-genders/chart-genders.component';
import { AddGenderAdminComponent } from './genders/add-gender-admin/add-gender-admin.component';
import { EditGenderAdminComponent } from './genders/edit-gender-admin/edit-gender-admin.component';
import { GendersListAdminComponent } from './genders/genders-list-admin/genders-list-admin.component';



@NgModule({
  declarations: [
    UsersListComponent,
    RegisterDoctorComponent,
    HospitalsListAdminComponent,
    AddHospitalAdminComponent,
    EditHospitalAdminComponent,
    SpecialtiesListAdminComponent,
    AddSpecialtyAdminComponent,
    EditSpecialtyAdminComponent,
    StatisticsComponent,
    ChartOfficesComponent,
    ChartAppointmentsComponent,
    ChartPatientsComponent,
    ChartGendersComponent,
    AddGenderAdminComponent,
    EditGenderAdminComponent,
    GendersListAdminComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
