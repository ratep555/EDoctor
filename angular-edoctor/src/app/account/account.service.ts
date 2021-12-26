import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Specialty } from '../shared/models/specialty';
import { Hospital } from '../shared/models/hospital';
import { Gender } from '../shared/models/gender';
import { ForgotPassword } from '../shared/models/forgotpassword';
import { ResetPassword } from '../shared/models/resetpassword';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  login(values: any) {
    return this.http.post(this.baseUrl + 'account/login', values)
     .pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  registerAsPatient(formData) {
    return this.http.post(this.baseUrl + 'account/register', formData).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  registerDoctor(values: any) {
    return this.http.post(this.baseUrl + 'account/registerdoctor', values).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  forgotPassword(forgotpassword: ForgotPassword) {
    return this.http.post(this.baseUrl + 'account/forgotpassword', forgotpassword);
  }

  resetPassword(resetpassword: ResetPassword) {
    return this.http.post(this.baseUrl + 'account/resetpassword', resetpassword);
  }

  getAllSpecialties() {
    return this.http.get<Specialty[]>(this.baseUrl + 'specialties');
  }

  getAllHospitals() {
    return this.http.get<Hospital[]>(this.baseUrl + 'hospitals/office');
  }

  getAllGenders() {
    return this.http.get<Gender[]>(this.baseUrl + 'patients/genders');
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  getDecodedToken(token) {
    return JSON.parse(atob(token.split('.')[1]));
  }

}










