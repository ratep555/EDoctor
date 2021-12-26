import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-add-gender-admin',
  templateUrl: './add-gender-admin.component.html',
  styleUrls: ['./add-gender-admin.component.css']
})
export class AddGenderAdminComponent implements OnInit {
  genderForm: FormGroup;

  constructor(private adminService: AdminService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createGenderForm();
  }

  createGenderForm() {
    this.genderForm = this.fb.group({
      id: [0],
      genderType: ['', [Validators.required]]
    });
  }

  onSubmit() {
    this.adminService.createGender(this.genderForm.value).subscribe(() => {
      this.router.navigateByUrl('admin/genderslistadmin');
    },
    error => {
      console.log(error);
    });
  }

}
