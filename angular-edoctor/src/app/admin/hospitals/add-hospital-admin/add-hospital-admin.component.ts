import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HospitalCreateEdit } from 'src/app/shared/models/hospital';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-add-hospital-admin',
  templateUrl: './add-hospital-admin.component.html',
  styleUrls: ['./add-hospital-admin.component.css']
})
export class AddHospitalAdminComponent implements OnInit {
  hospitalForm: FormGroup;

  constructor(private adminService: AdminService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createHospitalForm();
  }

  createHospitalForm() {
    this.hospitalForm = this.fb.group({
      id: [0],
      hospitalName: ['', [Validators.required]],
      street: ['', [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]]
    });
  }

  onSubmit() {
    this.adminService.createHospital(this.hospitalForm.value).subscribe(() => {
      this.resetForm(this.hospitalForm);
      this.router.navigateByUrl('admin/hospitalslistadmin');
    },
    error => {
      console.log(error);
    });
  }

   resetForm(form: FormGroup) {
    form.reset();
    this.adminService.formData = new HospitalCreateEdit();
  }

}
