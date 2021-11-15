import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MedicalRecord } from 'src/app/shared/models/medicalrecord';
import { Patient } from 'src/app/shared/models/patient';
import { MyParams } from 'src/app/shared/models/userparams';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-info-patient',
  templateUrl: './info-patient.component.html',
  styleUrls: ['./info-patient.component.css']
})
export class InfoPatientComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  patient: Patient;
  medicalrecords: MedicalRecord[];
  myParams = new MyParams();
  totalCount: number;

  sortOptions = [
    {name: 'Latest', value: 'dateDesc'},
    {name: 'Earliest', value: 'dateAsc'},
  ];

  constructor(private patientsService: PatientsService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadPatient();
    this.getMedicalrecords();
  }

  loadPatient() {
    return this.patientsService.getPatient(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(response => {
    this.patient = response;
    }, error => {
    console.log(error);
    });
    }

    getMedicalrecords() {
    this.patientsService
    .getMedicalrecordsForDoctorsPatient(+this.activatedRoute.snapshot.paramMap.get('id'), this.myParams)
    .subscribe(response => {
      this.medicalrecords = response.data;
      this.myParams.page = response.page;
      this.myParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

 onSearch() {
    this.myParams.query = this.searchTerm.nativeElement.value;
    this.getMedicalrecords();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.getMedicalrecords();
  }

  onSortSelected(sort: string) {
    this.myParams.sort = sort;
    this.getMedicalrecords();
  }

  onPageChanged(event: any) {
    if (this.myParams.page !== event) {
      this.myParams.page = event;
      this.getMedicalrecords();
    }
}

}

