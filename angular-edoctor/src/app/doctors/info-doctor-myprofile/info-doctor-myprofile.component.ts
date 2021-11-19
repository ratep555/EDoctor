import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Doctor } from 'src/app/shared/models/doctor';
import { User } from 'src/app/shared/models/user';
import { DoctorsService } from '../doctors.service';

@Component({
  selector: 'app-info-doctor-myprofile',
  templateUrl: './info-doctor-myprofile.component.html',
  styleUrls: ['./info-doctor-myprofile.component.css']
})
export class InfoDoctorMyprofileComponent implements OnInit {
  user: User;
  doctor: Doctor;

  constructor(private accountService: AccountService,
              private doctorsService: DoctorsService)
  { this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); }

  ngOnInit(): void {
    this.loadDoctor();
  }

  loadDoctor() {
    return this.doctorsService.getDoctorForMyProfile(this.user.userId)
    .subscribe(response => {
    this.doctor = response;
    }, error => {
    console.log(error);
    });
    }

}
