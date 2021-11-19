import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Patient } from 'src/app/shared/models/patient';
import { UserParams } from 'src/app/shared/models/userparams';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-patients-list-doctor',
  templateUrl: './patients-list-doctor.component.html',
  styleUrls: ['./patients-list-doctor.component.css']
})
export class PatientsListDoctorComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  patients: Patient[];
  userParams: UserParams;
  totalCount: number;

  sortOptions = [
    {name: 'Sort Alphabetical by Name', value: 'city'},
    {name: 'Sort Alphabetical by Name Descending', value: 'nameDesc'},
  ];

  constructor(private patientsService: PatientsService,
              private  router: Router)
  { this.userParams = this.patientsService.getUserParams(); }

  ngOnInit(): void {
    this.getAllPatientsOfDoctor();
  }

  getAllPatientsOfDoctor() {
    this.patientsService.setUserParams(this.userParams);
    this.patientsService.getAllPatientsOfDoctor(this.userParams)
    .subscribe(response => {
      this.patients = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  resetFilters() {
    this.userParams = this.patientsService.resetUserParams();
    this.getAllPatientsOfDoctor();
  }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getAllPatientsOfDoctor();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.patientsService.resetUserParams();
    this.getAllPatientsOfDoctor();
  }

  onSortSelected(sort: string) {
    this.userParams.sort = sort;
    this.getAllPatientsOfDoctor();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.patientsService.setUserParams(this.userParams);
      this.getAllPatientsOfDoctor();
    }
}


}
