import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Appointment, AppointmentCreateEdit } from 'src/app/shared/models/appointment';
import { AppointmentsService } from '../appointments.service';

@Component({
  selector: 'app-confirm-appointment-doctor',
  templateUrl: './confirm-appointment-doctor.component.html',
  styleUrls: ['./confirm-appointment-doctor.component.css']
})
export class ConfirmAppointmentDoctorComponent implements OnInit {
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
              patientId: [appointment.patientId],
              startDateAndTimeOfAppointment: [new Date(appointment.startDateAndTimeOfAppointment)],
              endDateAndTimeOfAppointment: [new Date(appointment.endDateAndTimeOfAppointment)],
              remarks: [appointment.remarks],
              status: [appointment.status]
            }));
      });
  }

  recordSubmit(fg: FormGroup) {
      this.appointmentsService.updateAppointment(fg.value).subscribe(
        (res: any) => {
          this.router.navigateByUrl('appointments/appointmentslistdoctor');
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
