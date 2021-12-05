import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[] = [];

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit() {
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
      street: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      city: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      country: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      mBO: [null, Validators.maxLength(9)],
      dateOfBirth: ['', Validators.required],
      phoneNumber: [null, Validators.required,
        Validators.minLength(5), Validators.maxLength(25)],
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
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/');
    }, error => {
      this.errors = error.errors;
      console.log(error);
    });
  }

}












