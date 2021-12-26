import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-edit-gender-admin',
  templateUrl: './edit-gender-admin.component.html',
  styleUrls: ['./edit-gender-admin.component.css']
})
export class EditGenderAdminComponent implements OnInit {
  genderForm: FormGroup;
  id: number;

  constructor(private adminService: AdminService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
  this.id = this.activatedRoute.snapshot.params['id'];

  this.genderForm = this.fb.group({
    id: [this.id],
    genderType: ['', [Validators.required]],
   });

  this.adminService.getGenderById(this.id)
  .pipe(first())
  .subscribe(x => this.genderForm.patchValue(x));
}

onSubmit() {
  if (this.genderForm.invalid) {
      return;
  }
  this.updateGender();
}

private updateGender() {
this.adminService.updateGender(this.id, this.genderForm.value)
    .pipe(first())
    .subscribe(() => {
      this.router.navigateByUrl('admin/genderslistadmin');
      }, error => {
        console.log(error);
      });
    }
}
