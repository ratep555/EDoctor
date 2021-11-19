import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MedicalRecord } from 'src/app/shared/models/medicalrecord';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-info-medicalrecord',
  templateUrl: './info-medicalrecord.component.html',
  styleUrls: ['./info-medicalrecord.component.css']
})
export class InfoMedicalrecordComponent implements OnInit {
  medicalrecord: MedicalRecord;

  constructor(private patientsService: PatientsService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMedicalRecord();
  }

  loadMedicalRecord() {
    return this.patientsService.getMedicalRecord(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.medicalrecord = response;
    }, error => {
    console.log(error);
    });
    }

}
