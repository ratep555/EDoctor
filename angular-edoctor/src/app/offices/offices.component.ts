import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Office } from '../shared/models/office';
import { Specialty } from '../shared/models/specialty';
import { UserParams } from '../shared/models/userparams';
import { OfficesService } from './offices.service';

@Component({
  selector: 'app-offices',
  templateUrl: './offices.component.html',
  styleUrls: ['./offices.component.css']
})
export class OfficesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('filter', {static: false}) filterTerm: ElementRef;
  offices: Office[];
  userParams: UserParams;
  totalCount: number;
  specialties: Specialty[];

  sortOptions = [
    {name: 'Sort Alphabetical by City', value: 'city'},
    {name: 'Initial Fee Price: Low to High', value: 'priceAsc'},
    {name: 'Initial Fee Price: High to Low', value: 'priceDesc'},
    {name: 'Follow Up Fee Price: Low to High', value: 'priceAscFollowUp'},
    {name: 'Follow Up Fee Price: High to Low', value: 'priceDescFollowUp'}
  ];

  constructor(private officesService: OfficesService,
              private  router: Router)
  {this.userParams = this.officesService.getUserParams(); }

  ngOnInit(): void {
    this.getOffices();
    this.getSpecialties();
  }

  getOffices() {
    this.officesService.setUserParams(this.userParams);
    this.officesService.getAllOffices(this.userParams)
    .subscribe(response => {
      this.offices = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  resetFilters() {
    this.userParams = this.officesService.resetUserParams();
    this.getOffices();
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
    this.getOffices();
    }


  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getOffices();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.officesService.resetUserParams();
    this.getOffices();
  }

  onReset1() {
    this.filterTerm.nativeElement.value = '';
    this.userParams = this.officesService.resetUserParams();
    this.getOffices();
  }

  onSortSelected(sort: string) {
    this.userParams.sort = sort;
    this.getOffices();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.officesService.setUserParams(this.userParams);
      this.getOffices();
    }
}


}
