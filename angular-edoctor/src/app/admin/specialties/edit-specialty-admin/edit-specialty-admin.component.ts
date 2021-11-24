import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-edit-specialty-admin',
  templateUrl: './edit-specialty-admin.component.html',
  styleUrls: ['./edit-specialty-admin.component.css']
})
export class EditSpecialtyAdminComponent implements OnInit {
  specialtyForm: FormGroup;
  id: number;

  constructor(private adminService: AdminService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
  this.id = this.activatedRoute.snapshot.params['id'];

  this.specialtyForm = this.fb.group({
    id: [this.id],
    specialtyName: ['', [Validators.required]],
   });

  this.adminService.getSpecialtyById(this.id)
  .pipe(first())
  .subscribe(x => this.specialtyForm.patchValue(x));
}

onSubmit() {
  if (this.specialtyForm.invalid) {
      return;
  }
  this.updateSpecialty();
}

private updateSpecialty() {
this.adminService.updateSpecialty(this.id, this.specialtyForm.value)
    .pipe(first())
    .subscribe(() => {
      this.router.navigateByUrl('admin/specialtieslistadmin');
      }, error => {
        console.log(error);
      });
    }
}
