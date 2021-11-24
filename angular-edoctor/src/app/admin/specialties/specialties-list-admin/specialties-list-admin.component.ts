import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Specialty } from 'src/app/shared/models/specialty';
import { MyParams } from 'src/app/shared/models/userparams';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { AdminService } from '../../admin.service';


@Component({
  selector: 'app-specialties-list-admin',
  templateUrl: './specialties-list-admin.component.html',
  styleUrls: ['./specialties-list-admin.component.css']
})
export class SpecialtiesListAdminComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  specialties: Specialty[];
  myParams = new MyParams();
  totalCount: number;

  constructor(private adminService: AdminService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getSpecialties();
  }

  getSpecialties() {
    this.adminService.getAllSpecialties(this.myParams)
    .subscribe(response => {
      this.specialties = response.data;
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
    this.getSpecialties();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.getSpecialties();
  }

  onPageChanged(event: any) {
    if (this.myParams.page !== event) {
      this.myParams.page = event;
      this.getSpecialties();
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
        this.adminService.deleteSpecialty(id)
    .subscribe(
      res => {
        this.getSpecialties();
        this.toastr.error('Deleted successfully!');
      }, err => { console.log(err);
       });
  }
});
}

}
