import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Appointment } from 'src/app/shared/models/appointment';
import { UserParams } from 'src/app/shared/models/userparams';
import { AppointmentsService } from '../appointments.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-appointments-list-doctor',
  templateUrl: './appointments-list-doctor.component.html',
  styleUrls: ['./appointments-list-doctor.component.css']
})
export class AppointmentsListDoctorComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('filter', {static: false}) filterTerm: ElementRef;
  appointments: Appointment[];
  userParams: UserParams;
  totalCount: number;

  sortOptions = [
    {name: 'Upcoming appointments', value: 'upcoming'},
    {name: 'All active', value: 'city'},
    {name: 'Available', value: 'available'},
    {name: 'Unconfirmed active', value: 'unconfirmed'},
    {name: 'Previous attended', value: 'previousattended'},
    {name: 'Previous non-attended', value: 'previousnonattended'},
    {name: 'All', value: 'all'},
  ];

  constructor(private appointmentsService: AppointmentsService,
              private  router: Router,
              private toastr: ToastrService) {
  this.userParams = this.appointmentsService.getUserParams();
  }

  ngOnInit(): void {
    this.getAppointments();
  }

  getAppointments() {
    this.appointmentsService.setUserParams(this.userParams);
    this.appointmentsService.getAllAppointmentsForDoctor(this.userParams)
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

  onFilter() {
    this.userParams.status = this.filterTerm.nativeElement.value;
    this.getAppointments();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.appointmentsService.resetUserParams();
    this.getAppointments();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.appointmentsService.setUserParams(this.userParams);
      this.getAppointments();
    }
}

onSortSelected(sort: string) {
  this.userParams.sort = sort;
  this.getAppointments();
}

onDelete(id: number) {
  Swal.fire({
    title: 'Are you sure want to terminate this appointment?',
    text: 'You will not be able to recover it afterwards!',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, terminate it!',
    confirmButtonColor: '#DD6B55',
    cancelButtonText: 'No, keep it'
  }).then((result) => {
    if (result.value) {
        this.appointmentsService.deleteAppointment(id)
    .subscribe(
      res => {
        this.getAppointments();
        this.toastr.error('Deleted successfully!');
      }, err => { console.log(err);
       });
  }
});
}
}
