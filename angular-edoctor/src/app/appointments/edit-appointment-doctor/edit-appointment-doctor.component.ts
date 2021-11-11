import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { AppointmentCreateEdit } from 'src/app/shared/models/appointment';
import { AppointmentsService } from '../appointments.service';

@Component({
  selector: 'app-edit-appointment-doctor',
  templateUrl: './edit-appointment-doctor.component.html',
  styleUrls: ['./edit-appointment-doctor.component.css']
})
export class EditAppointmentDoctorComponent implements OnInit {
  appointmentForms: FormArray = this.fb.array([]);
  officeList = [];
  id: number;

  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private appointmentsService: AppointmentsService) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.appointmentsService.getDoctorOffices()
    .subscribe(res => this.officeList = res as []);

    this.appointmentsService.getAppointmentById(this.id).subscribe(
      (appointment: AppointmentCreateEdit) => {
        this.appointmentForms.push(this.fb.group({
                  id: [this.id],
                  officeId: [appointment.officeId, Validators.required],
                  startDateAndTimeOfAppointment: [new Date(appointment.startDateAndTimeOfAppointment), Validators.required],
                  endDateAndTimeOfAppointment: [new Date(appointment.endDateAndTimeOfAppointment), Validators.required],
                }));
          });

  /*   this.appointmentForm = this.fb.group({
      id: [this.id],
      startDateAndTimeOfAppointment: ['', Validators.required],
      endDateAndTimeOfAppointment: ['', Validators.required],
      officeId: [0, Validators.min(1)],
    }); */

  /*   this.appointmentsService.getAppointmentById(this.id)
    .pipe(first())
    .subscribe(x => this.appointmentForm.patchValue(x)); */
  }

  recordSubmit(fg: FormGroup) {
    this.appointmentsService.updateAppointment(fg.value).subscribe(
      (res: any) => {
        this.router.navigateByUrl('appointments/appointmentslistdoctor');
      }, error => {
          console.log(error);
        });
      }


 /*  onSubmit() {
    if (this.appointmentForm.invalid) {
        return;
    }
    this.updateAppointment();
  }

  private updateAppointment() {
  this.appointmentsService.updateAppointment(this.id, this.appointmentForm.value)
      .pipe(first())
      .subscribe(() => {
        this.router.navigateByUrl('appointments/appointmentslistdoctor');
      }, error => {
          console.log(error);
        });
      } */


}






