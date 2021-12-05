import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CoordinatesMap } from 'src/app/shared/models/coordinate';
import { Office } from 'src/app/shared/models/office';
import { OfficesService } from '../offices.service';

@Component({
  selector: 'app-add-office-doctor',
  templateUrl: './add-office-doctor.component.html',
  styleUrls: ['./add-office-doctor.component.css']
})
export class AddOfficeDoctorComponent implements OnInit {
  officeForm: FormGroup;
  hospitalList = [];
  initialCoordinates: CoordinatesMap[] = [];
  model: Office;

  constructor(public officesService: OfficesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.officesService.getHospitals()
    .subscribe(res => this.hospitalList = res as []);

    this.createOfficeForm();
  }

  createOfficeForm() {
    this.officeForm = this.fb.group({
      street: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      city: [null, [Validators.required,
        Validators.minLength(2), Validators.maxLength(40)]],
      country: [null, [Validators.required,
        Validators.minLength(3), Validators.maxLength(60)]],
      initialExaminationFee: ['', [Validators.required]],
      followUpExaminationFee: ['', [Validators.required]],
      description: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(2000)]],
      longitude: ['', [Validators.required]],
      latitude: ['', [Validators.required]],
      hospitalId: [null],
      picture: ''
    });
  }

  onSubmit() {
    if (this.officeForm.get('hospitalId') === undefined) {
      this.officeForm.get('hospitalId').setValue(null);
    }
    this.officesService.createOffice(this.officeForm.value).subscribe(() => {
      this.router.navigateByUrl('offices/officeslistdoctor');
    },
    error => {
      console.log(error);
    });
  }

  onImageSelected(image){
    this.officeForm.get('picture').setValue(image);
  }

  onSelectedLocation(coordinates: CoordinatesMap) {
    this.officeForm.patchValue(coordinates);
 }

}

