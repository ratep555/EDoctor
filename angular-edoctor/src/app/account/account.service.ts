import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Specialty } from '../shared/models/specialty';
import { Hospital } from '../shared/models/hospital';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  login(values: any) {
    return this.http.post(this.baseUrl + 'accounts/login', values)
     .pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'accounts/register', values).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  registerDoctor(values: any) {
    return this.http.post(this.baseUrl + 'accounts/registerdoctor', values).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  getAllSpecialties() {
    return this.http.get<Specialty[]>(this.baseUrl + 'specialties');
  }

  getAllHospitals() {
    return this.http.get<Hospital[]>(this.baseUrl + 'hospitals');
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










