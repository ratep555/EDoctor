import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { PatientEdit } from 'src/app/shared/models/patient';
import { User } from 'src/app/shared/models/user';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-edit-patient',
  templateUrl: './edit-patient.component.html',
  styleUrls: ['./edit-patient.component.css']
})
export class EditPatientComponent implements OnInit {
  patientForms: FormArray = this.fb.array([]);
  user: User;
  id: number;

  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private activatedRoute: ActivatedRoute,
              private patientsService: PatientsService,
              private router: Router)
    { this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.patientsService.getPatient(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(
      (patient: PatientEdit) => {
      this.patientForms.push(this.fb.group({
        id: [this.id],
        applicationUserId: [this.user.userId],
        name: [patient.name, Validators.required],
        dateOfBirth: [new Date(patient.dateOfBirth), Validators.required],
        street: [patient.street, Validators.required],
        city: [patient.city, Validators.required],
        country: [patient.country, Validators.required],
        mBO: [patient.mbo],
        }));
      });
  }

  recordSubmit(fg: FormGroup) {
    this.patientsService.updatingPatientssProfile(fg.value).subscribe(
      (res: any) => {
        this.router.navigateByUrl('patients/infopatientmyprofile');
      }, error => {
          console.log(error);
        });
      }

}








