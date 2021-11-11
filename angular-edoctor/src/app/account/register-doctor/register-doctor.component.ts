import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register-doctor',
  templateUrl: './register-doctor.component.html',
  styleUrls: ['./register-doctor.component.css']
})
export class RegisterDoctorComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[] = [];
  nonSelectedSpecializations: MultipleSelectorModel[] = [];
  selectedSpecializations: MultipleSelectorModel[] = [];
  nonSelectedHospitals: MultipleSelectorModel[] = [];
  selectedHospitals: MultipleSelectorModel[] = [];

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit() {
    this.accountService.getAllSpecialties().subscribe(response => {
      this.nonSelectedSpecializations = response.map(specialty => {
        return  {key: specialty.id, value: specialty.specialtyName} as MultipleSelectorModel;
      });
    });

    this.accountService.getAllHospitals().subscribe(response => {
      this.nonSelectedHospitals = response.map(hospital => {
        return  {key: hospital.id, value: hospital.hospitalName} as MultipleSelectorModel;
      });
    });

    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      username: [null, [Validators.required]],
      startedPracticing: [null],
      resume: [null],
      specialtiesIds: [null],
      hospitalsIds: [null],
      email: [null,
        [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
      ],
      password: ['', [Validators.required,
        Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.compareValues('password')]]
    });
  }

  compareValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : {isEqual: true};
    };
  }

  onSubmit() {
    const specialtiesIds = this.selectedSpecializations.map(value => value.key);
    this.registerForm.get('specialtiesIds').setValue(specialtiesIds);

    const hospitalsIds = this.selectedHospitals.map(value => value.key);
    this.registerForm.get('hospitalsIds').setValue(hospitalsIds);

    this.accountService.registerDoctor(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/');
    }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }

}
