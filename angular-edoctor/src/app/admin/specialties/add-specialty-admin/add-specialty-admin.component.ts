import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SpecialtyCreateEdit } from 'src/app/shared/models/specialty';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-add-specialty-admin',
  templateUrl: './add-specialty-admin.component.html',
  styleUrls: ['./add-specialty-admin.component.css']
})
export class AddSpecialtyAdminComponent implements OnInit {
  specialtyForm: FormGroup;

  constructor(private adminService: AdminService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createSpecialtyForm();
  }

  createSpecialtyForm() {
    this.specialtyForm = this.fb.group({
      id: [0],
      specialtyName: ['', [Validators.required]]
    });
  }

  onSubmit() {
    this.adminService.createSpecialty(this.specialtyForm.value).subscribe(() => {
      this.resetForm(this.specialtyForm);
      this.router.navigateByUrl('admin/specialtieslistadmin');
    },
    error => {
      console.log(error);
    });
  }

   resetForm(form: FormGroup) {
    form.reset();
    this.adminService.formData1 = new SpecialtyCreateEdit();
  }

}
