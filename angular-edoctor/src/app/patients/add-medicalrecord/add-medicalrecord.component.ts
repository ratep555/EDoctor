import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppointmentsService } from 'src/app/appointments/appointments.service';
import { DoctorsService } from 'src/app/doctors/doctors.service';
import { Appointment } from 'src/app/shared/models/appointment';
import { MedicalrecordCreateEdit } from 'src/app/shared/models/medicalrecord';
import { Patient } from 'src/app/shared/models/patient';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-add-medicalrecord',
  templateUrl: './add-medicalrecord.component.html',
  styleUrls: ['./add-medicalrecord.component.css']
})
export class AddMedicalrecordComponent implements OnInit {
  appointment: Appointment;
  medicalrecordForm: FormGroup;

  constructor(private patientsService: PatientsService,
              private appointmentsService: AppointmentsService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadAppointment();

    this.createMedicalRecordForm();
  }

  loadAppointment() {
    return this.appointmentsService.getAppointmentByIdForReadonlyData(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.appointment = response;
    }, error => {
    console.log(error);
    });
    }

    createMedicalRecordForm() {
      this.medicalrecordForm = this.fb.group({
      anamnesisDiagnosisTherapy: ['', [Validators.required]],
      created: ['', Validators.required]
    });
  }

  onSubmit() {
    this.patientsService.formData.appointmentId = this.appointment.id;
    this.patientsService.createMedicalRecord(this.medicalrecordForm.value).subscribe(() => {
      this.router.navigateByUrl('patients/medicalrecordslistdoctor');
    },
    error => {
      console.log(error);
    });
  }

  }
