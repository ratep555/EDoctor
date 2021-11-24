import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-edit-hospital-admin',
  templateUrl: './edit-hospital-admin.component.html',
  styleUrls: ['./edit-hospital-admin.component.css']
})
export class EditHospitalAdminComponent implements OnInit {
  hospitalForm: FormGroup;
  id: number;

  constructor(private adminService: AdminService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
  this.id = this.activatedRoute.snapshot.params['id'];

  this.hospitalForm = this.fb.group({
    id: [this.id],
    hospitalName: ['', [Validators.required]],
    street: ['', [Validators.required]],
    city: ['', [Validators.required]],
    country: ['', [Validators.required]]
   });

  this.adminService.getHospitalById(this.id)
  .pipe(first())
  .subscribe(x => this.hospitalForm.patchValue(x));
}

onSubmit() {
  if (this.hospitalForm.invalid) {
      return;
  }
  this.updateHospital();
}

private updateHospital() {
this.adminService.updateHospital(this.id, this.hospitalForm.value)
    .pipe(first())
    .subscribe(() => {
      this.router.navigateByUrl('admin/hospitalslistadmin');
      }, error => {
        console.log(error);
      });
    }
}
