import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { OfficesService } from 'src/app/offices/offices.service';
import { AppointmentCreateEdit } from 'src/app/shared/models/appointment';
import { AppointmentsService } from '../appointments.service';

@Component({
  selector: 'app-add-appointment-doctor',
  templateUrl: './add-appointment-doctor.component.html',
  styleUrls: ['./add-appointment-doctor.component.css']
})
export class AddAppointmentDoctorComponent implements OnInit {
  appointmentForm: FormGroup;
  officeList = [];

  constructor(private appointmentsService: AppointmentsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.appointmentsService.getDoctorOffices()
    .subscribe(res => this.officeList = res as []);

    this.createAppointmentForm();
  }

  createAppointmentForm() {
    this.appointmentForm = this.fb.group({
      id: [0],
      startDateAndTimeOfAppointment: ['', Validators.required],
      endDateAndTimeOfAppointment: ['', Validators.required],
      officeId: [0, Validators.min(1)],
    });
  }

  onSubmit() {
    this.appointmentsService.createAppointment(this.appointmentForm.value).subscribe(() => {
      this.router.navigateByUrl('appointments/appointmentslistdoctor');
    },
    error => {
      console.log(error);
    });
  }

}












