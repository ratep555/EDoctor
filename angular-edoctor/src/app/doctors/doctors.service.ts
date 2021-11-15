import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { Doctor, DoctorEditDto, DoctorPutGetDto } from '../shared/models/doctor';
import { PaginationForDoctors } from '../shared/models/pagination';
import { User } from '../shared/models/user';
import { UserParams } from '../shared/models/userparams';

@Injectable({
  providedIn: 'root'
})
export class DoctorsService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;

  constructor(private http: HttpClient,
              private accountService: AccountService) {
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

  getAllDoctors(userParams: UserParams) {
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
    return this.http.get<PaginationForDoctors>(this.baseUrl + 'doctors', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getDoctor(id: number) {
    return this.http.get<Doctor>(this.baseUrl + 'doctors/' + id);
  }

  getDoctorForMyProfile(id: number) {
    return this.http.get<Doctor>(this.baseUrl + 'doctors/userid/' + id);
  }

  putGetDoctor(id: number): Observable<DoctorPutGetDto>{
    return this.http.get<DoctorPutGetDto>(this.baseUrl + 'doctors/putget/' + id);
  }

  updatingDoctorsProfile(id: number, doctorEditDTO: DoctorEditDto){
    const formData = this.BuildFormData(doctorEditDTO);
    return this.http.put(this.baseUrl + 'doctors/updatingdoctorsprofile/' + id, formData);
  }

  public rate(doctorId: number, rating: number){
    return this.http.post(this.baseUrl + 'ratings', {doctorId, rating});
  }

  private BuildFormData(doctor: DoctorEditDto): FormData {
    const formData = new FormData();
    formData.append('id', JSON.stringify(doctor.id));
    formData.append('applicationUserId', JSON.stringify(doctor.applicationUserId));
    if (doctor.name){
    formData.append('name', doctor.name);
    }
    if (doctor.resume){
    formData.append('resume', doctor.resume);
    }
    if (doctor.picture){
      formData.append('picture', doctor.picture);
    }
    if (doctor.specialtiesIds) {
    formData.append('specialtiesIds', JSON.stringify(doctor.specialtiesIds));
    }
    if (doctor.hospitalsIds) {
    formData.append('hospitalsIds', JSON.stringify(doctor.hospitalsIds));
    }
    return formData;
  }

}
