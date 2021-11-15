import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Doctor, DoctorPutGetDto } from 'src/app/shared/models/doctor';
import { Hospital } from 'src/app/shared/models/hospital';
import { Specialty } from 'src/app/shared/models/specialty';
import { DoctorsService } from '../doctors.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-info-doctor',
  templateUrl: './info-doctor.component.html',
  styleUrls: ['./info-doctor.component.css']
})
export class InfoDoctorComponent implements OnInit {
  doctor: Doctor;

  constructor(private doctorsService: DoctorsService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadDoctor();
  }

  loadDoctor() {
    return this.doctorsService.getDoctor(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.doctor = response;
    }, error => {
    console.log(error);
    });
    }

  onRating(rate: number){
    this.doctorsService.rate(this.doctor.id, rate).subscribe(() => {
     Swal.fire('Success', 'Your vote has been received', 'success');
     this.loadDoctor();
   });
 }


}
