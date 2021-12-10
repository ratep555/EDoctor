import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  patientForms: FormArray = this.fb.array([]);
  genderList = [];

  constructor(private fb: FormBuilder,
              private accountService: AccountService,
              private router: Router) { }

  ngOnInit() {
    this.accountService.getAllGenders()
    .subscribe(res => this.genderList = res as []);
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.patientForms.push(this.fb.group({
      firstName: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(30)]],
      lastName: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(60)]],
      username: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(20)]],
      street: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      city: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      country: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      mBO: [null, [Validators.maxLength(9)]],
      dateOfBirth: ['', [Validators.required]],
      genderId: [0, [Validators.min(1)]],
      phoneNumber: [null, [Validators.required,
        Validators.minLength(5), Validators.maxLength(25)]],
      email: [null,
        [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],
      ],
      password: ['', [Validators.required,
        Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.compareValues('password')]]
    }));
  }

  compareValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : {isEqual: true};
    };
  }

  recordSubmit(fg: FormGroup) {
    this.accountService.registerAsPatient(fg.value).subscribe(
      (res: any) => {
        this.router.navigateByUrl('/doctors');
      }, error => {
          console.log(error);
        });
      }
}











