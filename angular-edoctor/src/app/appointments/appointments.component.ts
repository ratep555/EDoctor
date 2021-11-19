import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { OfficesService } from '../offices/offices.service';
import { Appointment } from '../shared/models/appointment';
import { Specialty } from '../shared/models/specialty';
import { UserParams } from '../shared/models/userparams';
import { AppointmentsService } from './appointments.service';

@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrls: ['./appointments.component.css']
})
export class AppointmentsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('filter', {static: false}) filterTerm: ElementRef;
  appointments: Appointment[];
  userParams: UserParams;
  totalCount: number;
  specialties: Specialty[];

  sortOptions = [
    {name: 'Earliest', value: 'city'},
    {name: 'Latest', value: 'dateDesc'}
  ];

  constructor(private appointmentsService: AppointmentsService,
              private officesService: OfficesService,
              private  router: Router)
  {this.userParams = this.appointmentsService.getUserParams(); }

  ngOnInit(): void {
    this.getAppointments();
    this.getSpecialties();
  }

  getAppointments() {
    this.appointmentsService.setUserParams(this.userParams);
    this.appointmentsService.getAllAvailableUpcomingAppointments(this.userParams)
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

  getSpecialties() {
    this.officesService.getSpecialtiesAttributedToDoctors().subscribe(response => {
    this.specialties = response;
    }, error => {
    console.log(error);
    });
    }

  onSpecialtySelected(specialtyId: number) {
    this.userParams.specialtyId = specialtyId;
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

    onReset1() {
      this.filterTerm.nativeElement.value = '';
      this.userParams = this.appointmentsService.resetUserParams();
      this.getAppointments();
    }

    onSortSelected(sort: string) {
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

  }
