import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MedicalRecord } from 'src/app/shared/models/medicalrecord';
import { UserParams } from 'src/app/shared/models/userparams';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-medicalrecords-list-patient',
  templateUrl: './medicalrecords-list-patient.component.html',
  styleUrls: ['./medicalrecords-list-patient.component.css']
})
export class MedicalrecordsListPatientComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  medicalrecords: MedicalRecord[];
  userParams: UserParams;
  totalCount: number;

  sortOptions = [
    {name: 'Latest', value: 'dateDesc'},
    {name: 'Earliest', value: 'dateAsc'},
  ];

  constructor(private patientsService: PatientsService,
              private  router: Router)
{this.userParams = this.patientsService.getUserParams(); }

  ngOnInit(): void {
    this.getMedicalRecords();
  }

  getMedicalRecords() {
    this.patientsService.setUserParams(this.userParams);
    this.patientsService.getAllMedicalRecordsForPatientAsUser(this.userParams)
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

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getMedicalRecords();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
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
      this.patientsService.setUserParams(this.userParams);
    }
}

}
