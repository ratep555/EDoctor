import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DoctorsService } from 'src/app/doctors/doctors.service';
import { MedicalrecordCreateEdit } from 'src/app/shared/models/medicalrecord';
import { Patient } from 'src/app/shared/models/patient';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-add-medicalrecord',
  templateUrl: './add-medicalrecord.component.html',
  styleUrls: ['./add-medicalrecord.component.css']
})
export class AddMedicalrecordComponent implements OnInit {
  patient: Patient;
  officeList = [];
  medicalrecordForm: FormGroup;

  constructor(private patientsService: PatientsService,
              private doctorsService: DoctorsService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadPatient();

    this.doctorsService.getAllOfficesForDoctorByUserId()
    .subscribe(res => this.officeList = res as []);

    this.createMedicalRecordForm();
  }

  loadPatient() {
    return this.patientsService.getPatient(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.patient = response;
    }, error => {
    console.log(error);
    });
    }

    createMedicalRecordForm() {
      this.medicalrecordForm = this.fb.group({
      anamnesisDiagnosisTherapy: ['', [Validators.required]],
      officeId: [0, Validators.min(1)],
      created: ['', Validators.required]
    });
  }

  onSubmit() {
    this.patientsService.formData.patientId = this.patient.id;
    this.patientsService.createMedicalRecord(this.medicalrecordForm.value).subscribe(() => {
      this.resetForm(this.medicalrecordForm);
      this.router.navigateByUrl('appointments/appointmentslistdoctor');
    },
    error => {
      console.log(error);
    });
  }

  resetForm(form: FormGroup) {
    form.reset();
    this.patientsService.formData = new MedicalrecordCreateEdit();
  }
  }
