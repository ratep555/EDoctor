import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Office } from 'src/app/shared/models/office';
import { UserParams } from 'src/app/shared/models/userparams';
import { OfficesService } from '../offices.service';

@Component({
  selector: 'app-offices-list-doctor',
  templateUrl: './offices-list-doctor.component.html',
  styleUrls: ['./offices-list-doctor.component.css']
})
export class OfficesListDoctorComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  offices: Office[];
  userParams: UserParams;
  totalCount: number;

  sortOptions = [
    {name: 'Sort Alphabetical by City', value: 'city'},
    {name: 'Initial Fee Price: Low to High', value: 'priceAsc'},
    {name: 'Initial Fee Price: High to Low', value: 'priceDesc'}
  ];

  constructor(private officesService: OfficesService,
              private  router: Router) {
    this.userParams = this.officesService.getUserParams();
  }

  ngOnInit(): void {
    this.getOffices();
  }

  getOffices() {
    this.officesService.setUserParams(this.userParams);
    this.officesService.getAllOfficesForDoctor(this.userParams)
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

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getOffices();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
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






