import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { Appointment, AppointmentCreateEdit } from '../shared/models/appointment';
import { Office } from '../shared/models/office';
import { PaginationForAppointments } from '../shared/models/pagination';
import { User } from '../shared/models/user';
import { UserParams } from '../shared/models/userparams';

@Injectable({
  providedIn: 'root'
})
export class AppointmentsService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;
  formData: AppointmentCreateEdit = new AppointmentCreateEdit();

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take (1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
    });
  }

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getAllAvailableUpcomingAppointments(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.specialtyId !== 0) {
      params = params.append('specialtyId', userParams.specialtyId.toString());
    }
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('sort', userParams.sort);
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<PaginationForAppointments>(this.baseUrl + 'appointments', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }


  getAllAppointmentsForDoctor(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('sort', userParams.sort);
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<PaginationForAppointments>(this.baseUrl + 'appointments/singledoctor', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  createAppointment(formData) {
    return this.http.post(this.baseUrl + 'appointments', formData);
  }

  updateAppointment(formData) {
    return this.http.put(this.baseUrl + 'appointments/' + formData.id, formData);
  }

  getAppointmentById(id: number) {
    return this.http.get<AppointmentCreateEdit>(this.baseUrl + 'appointments/' + id);
  }

  getDoctorOffices() {
    return this.http.get<Office[]>(this.baseUrl + 'appointments/offices');
  }
}




