import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Doctor } from 'src/app/shared/models/doctor';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { User } from 'src/app/shared/models/user';
import { DoctorsService } from '../doctors.service';

@Component({
  selector: 'app-edit-doctor',
  templateUrl: './edit-doctor.component.html',
  styleUrls: ['./edit-doctor.component.css']
})
export class EditDoctorComponent implements OnInit {
  user: User;
  model: Doctor;
  registerForm: FormGroup;
  nonSelectedSpecialties: MultipleSelectorModel[] = [];
  selectedSpecialties: MultipleSelectorModel[] = [];
  nonSelectedHospitals: MultipleSelectorModel[] = [];
  selectedHospitals: MultipleSelectorModel[] = [];
  id: number;

  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private activatedRoute: ActivatedRoute,
              private doctorsService: DoctorsService,
              private router: Router)
              { this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.activatedRoute.params.subscribe(params => {
    this.doctorsService.putGetDoctor(params.id).subscribe(putGetDTO => {
      this.model = putGetDTO.doctor;

      this.selectedSpecialties = putGetDTO.selectedSpecialties.map(specialty => {
        return {key: specialty.id, value: specialty.specialtyName} as MultipleSelectorModel;
      });
      this.nonSelectedSpecialties = putGetDTO.nonSelectedSpecialties.map(specialty => {
        return {key: specialty.id, value: specialty.specialtyName} as MultipleSelectorModel;
      });

      this.selectedHospitals = putGetDTO.selectedHospitals.map(hospital => {
        return {key: hospital.id, value: hospital.hospitalName} as MultipleSelectorModel;
      });
      this.nonSelectedHospitals = putGetDTO.nonSelectedHospitals.map(hospital => {
        return {key: hospital.id, value: hospital.hospitalName} as MultipleSelectorModel;
      });
    });
  });

    this.createRegisterForm();

    this.doctorsService.getDoctor(+this.activatedRoute.snapshot.paramMap.get('id'))
    .pipe(first())
    .subscribe(x => this.registerForm.patchValue(x));
}

createRegisterForm() {
  this.registerForm = this.fb.group({
    id: [this.id],
    applicationUserId: [this.user.userId],
    name: [null, [Validators.required,
      Validators.minLength(5), Validators.maxLength(80)]],
    resume: [null, Validators.maxLength(5000)],
    qualifications: [null, Validators.maxLength(1000)],
    specialtiesIds: [null],
    hospitalsIds: [null],
    picture: ''
  });
}

onSubmit() {
  const specialtiesIds = this.selectedSpecialties.map(value => value.key);
  this.registerForm.get('specialtiesIds').setValue(specialtiesIds);

  const hospitalsIds = this.selectedHospitals.map(value => value.key);
  this.registerForm.get('hospitalsIds').setValue(hospitalsIds);

  this.doctorsService.updatingDoctorsProfile(this.id, this.registerForm.value).subscribe(() => {
    this.router.navigateByUrl('/doctors/infodoctormyprofile');
  }, error => {
    console.log(error);
  });
}

onImageSelected(image){
  this.registerForm.get('picture').setValue(image);
}

}


