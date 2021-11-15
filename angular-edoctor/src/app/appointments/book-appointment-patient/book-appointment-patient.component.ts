import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Appointment, AppointmentCreateEdit } from 'src/app/shared/models/appointment';
import { AppointmentsService } from '../appointments.service';

@Component({
  selector: 'app-book-appointment-patient',
  templateUrl: './book-appointment-patient.component.html',
  styleUrls: ['./book-appointment-patient.component.css']
})
export class BookAppointmentPatientComponent implements OnInit {
  appointmentForms: FormArray = this.fb.array([]);
  id: number;
  appointment: Appointment;

  constructor(private appointmentsService: AppointmentsService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.loadAppointment();

    this.appointmentsService.getAppointmentById(this.id).subscribe(
  (appointment: AppointmentCreateEdit) => {
    this.appointmentForms.push(this.fb.group({
              id: [this.id],
              officeId: [appointment.officeId],
              startDateAndTimeOfAppointment: [new Date(appointment.startDateAndTimeOfAppointment), Validators.required],
              endDateAndTimeOfAppointment: [new Date(appointment.endDateAndTimeOfAppointment), Validators.required],
              remarks: [appointment.remarks]
            }));
      });
  }

  recordSubmit(fg: FormGroup) {
      this.appointmentsService.bookAppointmentByPatient(fg.value).subscribe(
        (res: any) => {
          this.router.navigateByUrl('appointments');
        }, error => {
            console.log(error);
          });
        }

    loadAppointment() {
      return this.appointmentsService
      .getAppointmentByIdForReadonlyData(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(response => {
        this.appointment = response;
      }, error => {
        console.log(error);
      });
    }

}
