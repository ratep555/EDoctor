import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { Hospital } from '../shared/models/hospital';
import { Office, OfficeCreateEdit, OfficeCreateEdit1 } from '../shared/models/office';
import { PaginationForOffices } from '../shared/models/pagination';
import { Specialty } from '../shared/models/specialty';
import { User } from '../shared/models/user';
import { UserParams } from '../shared/models/userparams';

@Injectable({
  providedIn: 'root'
})
export class OfficesService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;
  formData: OfficeCreateEdit = new OfficeCreateEdit();

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

  getAllOffices(userParams: UserParams) {
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
    return this.http.get<PaginationForOffices>(this.baseUrl + 'offices', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getAllOfficesForDoctor(userParams: UserParams) {
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
    return this.http.get<PaginationForOffices>(this.baseUrl + 'offices/singledoctor', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getOfficeById(id: number) {
    return this.http.get<Office>(this.baseUrl + 'offices/' + id);
  }

  createOffice(office: OfficeCreateEdit1) {
    const formData = this.BuildFormData(office);
    return this.http.post(this.baseUrl + 'offices', formData);
  }

  updateOffice(id: number, office: OfficeCreateEdit1){
    const formData = this.BuildFormData1(office);
    return this.http.put(this.baseUrl + 'offices/' + id, formData);
  }

  getHospitals() {
    return this.http.get<Hospital[]>(this.baseUrl + 'offices/hospitals');
  }

  getSpecialtiesAttributedToDoctors() {
    return this.http.get<Specialty[]>(this.baseUrl + 'specialties/attributedtodoctors');
  }

  private BuildFormData(office: OfficeCreateEdit1): FormData {
    const formData = new FormData();
    formData.append('initialExaminationFee', JSON.stringify(office.initialExaminationFee));
    formData.append('followUpExaminationFee', JSON.stringify(office.followUpExaminationFee));
    formData.append('hospitalId', JSON.stringify(office.hospitalId));
    if (office.street){
    formData.append('street', office.street);
    }
    if (office.city){
    formData.append('city', office.city);
    }
    if (office.country){
    formData.append('country', office.country);
    }
    if (office.description){
    formData.append('description', office.description);
    }
    if (office.picture){
      formData.append('picture', office.picture);
    }
    if (office.latitude) {
    formData.append('latitude', JSON.stringify(office.latitude));
    }
    if (office.longitude) {
    formData.append('longitude', JSON.stringify(office.longitude));
    }
    return formData;
  }

  private BuildFormData1(office: OfficeCreateEdit1): FormData {
    const formData = new FormData();
    formData.append('id', JSON.stringify(office.id));
    formData.append('initialExaminationFee', JSON.stringify(office.initialExaminationFee));
    formData.append('followUpExaminationFee', JSON.stringify(office.followUpExaminationFee));
    formData.append('hospitalId', JSON.stringify(office.hospitalId));
    if (office.street){
    formData.append('street', office.street);
    }
    if (office.city){
    formData.append('city', office.city);
    }
    if (office.country){
    formData.append('country', office.country);
    }
    if (office.description){
    formData.append('description', office.description);
    }
    if (office.picture){
      formData.append('picture', office.picture);
    }
    if (office.latitude) {
    formData.append('latitude', JSON.stringify(office.latitude));
    }
    if (office.longitude) {
    formData.append('longitude', JSON.stringify(office.longitude));
    }
    return formData;
  }


}












