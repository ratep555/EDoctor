import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Gender } from 'src/app/shared/models/gender';
import { MyParams } from 'src/app/shared/models/userparams';
import { AdminService } from '../../admin.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';



@Component({
  selector: 'app-genders-list-admin',
  templateUrl: './genders-list-admin.component.html',
  styleUrls: ['./genders-list-admin.component.css']
})
export class GendersListAdminComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  genders: Gender[];
  myParams = new MyParams();
  totalCount: number;

  constructor(private adminService: AdminService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getGenders();
  }

  getGenders() {
    this.adminService.getAllGenders(this.myParams)
    .subscribe(response => {
      this.genders = response.data;
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
    this.getGenders();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.getGenders();
  }

  onPageChanged(event: any) {
    if (this.myParams.page !== event) {
      this.myParams.page = event;
      this.getGenders();
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
        this.adminService.deleteGender(id)
    .subscribe(
      res => {
        this.getGenders();
        this.toastr.error('Deleted successfully!');
      }, err => { console.log(err);
       });
  }
});
}

}
