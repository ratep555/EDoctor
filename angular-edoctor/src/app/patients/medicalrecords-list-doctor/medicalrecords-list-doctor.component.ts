import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DoctorsService } from 'src/app/doctors/doctors.service';
import { MedicalRecord } from 'src/app/shared/models/medicalrecord';
import { Office } from 'src/app/shared/models/office';
import { UserParams } from 'src/app/shared/models/userparams';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-medicalrecords-list-doctor',
  templateUrl: './medicalrecords-list-doctor.component.html',
  styleUrls: ['./medicalrecords-list-doctor.component.css']
})
export class MedicalrecordsListDoctorComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('filter', {static: false}) filterTerm: ElementRef;
  medicalrecords: MedicalRecord[];
  userParams: UserParams;
  totalCount: number;
  offices: Office[];

  sortOptions = [
    {name: 'Latest', value: 'dateDesc'},
    {name: 'Earliest', value: 'dateAsc'},
    {name: 'Hospital', value: 'hospital'},
    {name: 'Private', value: 'private'},
  ];

  constructor(private patientsService: PatientsService,
              private doctorsService: DoctorsService,
              private  router: Router)
  {this.userParams = this.patientsService.getUserParams(); }


  ngOnInit(): void {
    this.getMedicalRecords();
    this.getOffices();
  }

  getMedicalRecords() {
    this.patientsService.setUserParams(this.userParams);
    this.patientsService.getMedicalRecordsForAllPatientsOfDoctor(this.userParams)
    .subscribe(response => {
      this.medicalrecords = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  getOffices() {
    this.doctorsService.getAllOfficesForDoctorByUserId().subscribe(response => {
    this.offices = response;
    }, error => {
    console.log(error);
    });
    }

  onOfficeSelected(officeId: number) {
    this.userParams.officeId = officeId;
    this.getMedicalRecords();
    }


  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getMedicalRecords();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.patientsService.resetUserParams();
    this.getMedicalRecords();
  }

  onReset1() {
    this.filterTerm.nativeElement.value = '';
    this.userParams = this.patientsService.resetUserParams();
    this.getMedicalRecords();
  }

  onSortSelected(sort: string) {
    this.userParams.sort = sort;
    this.getMedicalRecords();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.doctorsService.setUserParams(this.userParams);
      this.getOffices();
    }
}

}
