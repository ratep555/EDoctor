import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Patient } from 'src/app/shared/models/patient';
import { User } from 'src/app/shared/models/user';
import { PatientsService } from '../patients.service';

@Component({
  selector: 'app-info-patient-myprofile',
  templateUrl: './info-patient-myprofile.component.html',
  styleUrls: ['./info-patient-myprofile.component.css']
})
export class InfoPatientMyprofileComponent implements OnInit {
  user: User;
  patient: Patient;

  constructor(private accountService: AccountService,
              private patientsService: PatientsService)
  { this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); }

  ngOnInit(): void {
    this.loadPatient();
  }

  loadPatient() {
    return this.patientsService.getPatientForMyProfile(this.user.userId)
    .subscribe(response => {
    this.patient = response;
    }, error => {
    console.log(error);
    });
    }

}
