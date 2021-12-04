import { formatDate } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Appointment } from 'src/app/shared/models/appointment';
import { UserParams } from 'src/app/shared/models/userparams';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { AppointmentsService } from '../appointments.service';

@Component({
  selector: 'app-appointments-list-patient',
  templateUrl: './appointments-list-patient.component.html',
  styleUrls: ['./appointments-list-patient.component.css']
})
export class AppointmentsListPatientComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  appointments: Appointment[];
  userParams: UserParams;
  totalCount: number;
  currentDate: Date = new Date();

  sortOptions1 = [
    {name: 'Upcoming Appointments', value: 'city'},
    {name: 'Pending', value: 'pending'},
    {name: 'Previous', value: 'previous'},
    {name: 'All', value: 'all'},
  ];

  sortOptions = [
    {name: 'Sort by Start Date Ascending', value: 'dateAsc'},
    {name: 'Sort by Start Date Descending', value: 'dateDesc'}
  ];

  constructor(private appointmentsService: AppointmentsService,
              private  router: Router) {
  this.userParams = this.appointmentsService.getUserParams();
  }

  ngOnInit(): void {
    this.getAppointments();
  }

  getAppointments() {
    this.appointmentsService.setUserParams(this.userParams);
    this.appointmentsService.getAllAppointmentsForPatient(this.userParams)
    .subscribe(response => {
      this.appointments = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  resetFilters() {
    this.userParams = this.appointmentsService.resetUserParams();
    this.getAppointments();
  }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getAppointments();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.appointmentsService.resetUserParams();
    this.getAppointments();
  }

  onSortSelected(sort: string) {
    this.userParams.sort = sort;
    this.getAppointments();
  }

  onSortSelected1(sort: string) {
    this.userParams.sort = sort;
    this.getAppointments();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.appointmentsService.setUserParams(this.userParams);
      this.getAppointments();
    }
}

cancelAppointment(id: number) {
  Swal.fire({
    title: 'Are you sure want to cancel this appointment?',
    text: 'You can book it again until it is booked by other patient.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, cancel it!',
    confirmButtonColor: '#DD6B55',
    cancelButtonText: 'No, forget about it'
  }).then((result) => {
    if (result.value) {
    this.appointmentsService.cancelAppointment(id)
      .subscribe(
        res => {
          this.getAppointments();
        },
        err => { console.log(err);
        });
      }
    });
    }
}
