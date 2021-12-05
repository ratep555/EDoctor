import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MedicalRecord, MedicalrecordCreateEdit } from 'src/app/shared/models/medicalrecord';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-edit-medicalrecord',
  templateUrl: './edit-medicalrecord.component.html',
  styleUrls: ['./edit-medicalrecord.component.css']
})
export class EditMedicalrecordComponent implements OnInit {
  recordForms: FormArray = this.fb.array([]);
  medicalrecord: MedicalRecord;
  id: number;

  constructor(private patientsService: PatientsService,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.loadMedicalRecord();

    this.patientsService.getMedicalRecordForEditing(this.id).subscribe(
    (record: MedicalrecordCreateEdit) => {
    this.recordForms.push(this.fb.group({
      appointmentId: [this.id],
      anamnesisDiagnosisTherapy: [record.anamnesisDiagnosisTherapy, [Validators.required]],
      created: [new Date(record.created), Validators.required],
      }));
    });
  }

  loadMedicalRecord() {
    return this.patientsService.getMedicalRecord(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(response => {
    this.medicalrecord = response;
    }, error => {
    console.log(error);
    });
    }

  recordSubmit(fg: FormGroup) {
    this.patientsService.formData.appointmentId = this.medicalrecord.appointmentId;
    this.patientsService.updateMedicalRecord(fg.value).subscribe(
      (res: any) => {
        this.router.navigateByUrl('patients/medicalrecordslistdoctor');
      }, error => {
          console.log(error);
        });
      }

}
