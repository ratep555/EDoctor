import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Marker } from 'leaflet';
import { first } from 'rxjs/operators';
import { CoordinatesMap } from 'src/app/shared/models/coordinate';
import { OfficesService } from '../offices.service';

@Component({
  selector: 'app-edit-office-doctor',
  templateUrl: './edit-office-doctor.component.html',
  styleUrls: ['./edit-office-doctor.component.css']
})
export class EditOfficeDoctorComponent implements OnInit {
  officeForm: FormGroup;
  hospitalList = [];
  id: number;
  initialCoordinates: CoordinatesMap[] = [];
  layers: Marker<any>[] = [];

  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private officesService: OfficesService) { }

ngOnInit(): void {
  this.id = this.activatedRoute.snapshot.params['id'];

  this.officesService.getHospitals()
  .subscribe(res => this.hospitalList = res as []);

  this.officeForm = this.fb.group({
    id: [this.id],
    street: ['', [Validators.required]],
    city: ['', [Validators.required]],
    country: ['', [Validators.required]],
    initialExaminationFee: ['', [Validators.required]],
    followUpExaminationFee: ['', [Validators.required]],
    description: ['', [Validators.required]],
    longitude: ['', [Validators.required]],
    latitude: ['', [Validators.required]],
    hospitalId: [null]
  });

  this.officesService.getOfficeById(this.id)
  .pipe(first())
  .subscribe(x => this.officeForm.patchValue(x));
}

onSubmit() {
  if (this.officeForm.invalid) {
      return;
  }
  this.updateOffice();
}

private updateOffice() {
this.officesService.updateOffice(this.id, this.officeForm.value)
    .pipe(first())
    .subscribe(() => {
      this.router.navigateByUrl('offices/officeslistdoctor');
      }, error => {
        console.log(error);
      });
    }

    onSelectedLocation(coordinates: CoordinatesMap) {
      this.officeForm.patchValue(coordinates);
      this.initialCoordinates.push({latitude: this.officesService.formData.latitude,
        longitude: this.officesService.formData.longitude});
   }
}
