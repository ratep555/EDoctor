import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { OfficesService } from '../offices/offices.service';
import { Doctor } from '../shared/models/doctor';
import { Specialty } from '../shared/models/specialty';
import { UserParams } from '../shared/models/userparams';
import { DoctorsService } from './doctors.service';

@Component({
  selector: 'app-doctors',
  templateUrl: './doctors.component.html',
  styleUrls: ['./doctors.component.css']
})
export class DoctorsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('filter', {static: false}) filterTerm: ElementRef;
  doctors: Doctor[];
  userParams: UserParams;
  totalCount: number;
  specialties: Specialty[];

  sortOptions = [
    {name: 'All Doctors', value: 'city'},
    {name: 'Hospital and Private Doctors', value: 'hospitalandprivate'},
    {name: 'Hospital Doctors', value: 'hospital'},
    {name: 'Private Doctors', value: 'private'},
    {name: 'Highest Rated', value: 'rateDesc'},
    {name: 'Lowest Rated', value: 'rateAsc'},
    {name: 'Most Experienced', value: 'practicingAsc'},
  ];

  constructor(private doctorsService: DoctorsService,
              private officesService: OfficesService,
              private  router: Router) {
              this.userParams = this.doctorsService.getUserParams();
               }

  ngOnInit(): void {
    this.getDoctors();
    this.getSpecialties();
  }

  getDoctors() {
    this.doctorsService.setUserParams(this.userParams);
    this.doctorsService.getAllDoctors(this.userParams)
    .subscribe(response => {
      this.doctors = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
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
    this.getDoctors();
    }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getDoctors();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.doctorsService.resetUserParams();
    this.getDoctors();
  }

  onReset1() {
    this.filterTerm.nativeElement.value = '';
    this.userParams = this.doctorsService.resetUserParams();
    this.getDoctors();
  }

  onSortSelected(sort: string) {
    this.userParams.sort = sort;
    this.getDoctors();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.doctorsService.setUserParams(this.userParams);
      this.getDoctors();
    }
}

}
