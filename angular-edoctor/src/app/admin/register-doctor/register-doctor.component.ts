import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/account/account.service';
import { MultipleSelectorModel } from 'src/app/shared/models/multiple-selector.model';

@Component({
  selector: 'app-register-doctor',
  templateUrl: './register-doctor.component.html',
  styleUrls: ['./register-doctor.component.css']
})
export class RegisterDoctorComponent implements OnInit {
  registerForm: FormGroup;
  nonSelectedSpecializations: MultipleSelectorModel[] = [];
  selectedSpecializations: MultipleSelectorModel[] = [];
  nonSelectedHospitals: MultipleSelectorModel[] = [];
  selectedHospitals: MultipleSelectorModel[] = [];

  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private router: Router) { }

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
      firstName: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(30)]],
      lastName: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(60)]],
      username: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(20)]],
      startedPracticing: [null],
      resume: [null, Validators.maxLength(5000)],
      qualifications: [null, Validators.maxLength(1000)],
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
      this.router.navigateByUrl('admin/userslist');
    }, error => {
      console.log(error);
    });
  }

}
