import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Hospital } from 'src/app/shared/models/hospital';
import { MyParams } from 'src/app/shared/models/userparams';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { AdminService } from '../../admin.service';


@Component({
  selector: 'app-hospitals-list-admin',
  templateUrl: './hospitals-list-admin.component.html',
  styleUrls: ['./hospitals-list-admin.component.css']
})
export class HospitalsListAdminComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  hospitals: Hospital[];
  myParams = new MyParams();
  totalCount: number;

  constructor(private adminService: AdminService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getHospitals();
  }

  getHospitals() {
    this.adminService.getAllHospitals(this.myParams)
    .subscribe(response => {
      this.hospitals = response.data;
      this.myParams.page = response.page;
      this.myParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  onSearch() {
    this.myParams.query = this.searchTerm.nativeElement.value;
    this.getHospitals();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.getHospitals();
  }

  onPageChanged(event: any) {
    if (this.myParams.page !== event) {
      this.myParams.page = event;
      this.getHospitals();
    }
}

onDelete(id: number) {
  Swal.fire({
    title: 'Are you sure want to delete this record?',
    text: 'You will not be able to recover it afterwards!',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, delete it!',
    confirmButtonColor: '#DD6B55',
    cancelButtonText: 'No, keep it'
  }).then((result) => {
    if (result.value) {
        this.adminService.deleteHospital(id)
    .subscribe(
      res => {
        this.getHospitals();
        this.toastr.error('Deleted successfully!');
      }, err => { console.log(err);
       });
  }
});
}

}

