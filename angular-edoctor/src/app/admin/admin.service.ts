import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MyParams } from '../shared/models/userparams';
import { PaginationForHospitals, PaginationForSpecialties, PaginationForUsers } from '../shared/models/pagination';
import { map } from 'rxjs/operators';
import { Hospital, HospitalCreateEdit } from '../shared/models/hospital';
import { Specialty, SpecialtyCreateEdit } from '../shared/models/specialty';
import { Statistics } from '../shared/models/statistics';


@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  formData: HospitalCreateEdit = new HospitalCreateEdit();
  formData1: SpecialtyCreateEdit = new SpecialtyCreateEdit();

  constructor(private http: HttpClient) { }

  getUsers(myparams: MyParams) {
    let params = new HttpParams();
    if (myparams.query) {
      params = params.append('query', myparams.query);
    }
    params = params.append('page', myparams.page.toString());
    params = params.append('pageCount', myparams.pageCount.toString());
    return this.http.get<PaginationForUsers>(this.baseUrl + 'admin', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getAllHospitals(myparams: MyParams) {
    let params = new HttpParams();
    if (myparams.query) {
      params = params.append('query', myparams.query);
    }
    params = params.append('page', myparams.page.toString());
    params = params.append('pageCount', myparams.pageCount.toString());
    return this.http.get<PaginationForHospitals>(this.baseUrl + 'hospitals/adminlist', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getAllSpecialties(myparams: MyParams) {
    let params = new HttpParams();
    if (myparams.query) {
      params = params.append('query', myparams.query);
    }
    params = params.append('page', myparams.page.toString());
    params = params.append('pageCount', myparams.pageCount.toString());
    return this.http.get<PaginationForSpecialties>(this.baseUrl + 'specialties/adminlist', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  createHospital(formData) {
    return this.http.post(this.baseUrl + 'hospitals', formData);
  }

  updateHospital(id: number, params: any) {
    return this.http.put(this.baseUrl + 'hospitals/' + id, params);
  }

  getHospitalById(id: number) {
    return this.http.get<Hospital>(this.baseUrl + 'hospitals/' + id);
  }

  deleteHospital(id: number) {
    return this.http.delete(this.baseUrl + 'hospitals/' + id);
}

  createSpecialty(formData1) {
    return this.http.post(this.baseUrl + 'specialties', formData1);
  }

  updateSpecialty(id: number, params: any) {
    return this.http.put(this.baseUrl + 'specialties/' + id, params);
  }

  getSpecialtyById(id: number) {
    return this.http.get<Specialty>(this.baseUrl + 'specialties/' + id);
  }

  deleteSpecialty(id: number) {
    return this.http.delete(this.baseUrl + 'specialties/' + id);
}

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }

  unlockUser(userId: number) {
    return this.http.put(this.baseUrl + 'admin/unlock/' + userId, {});
}

  lockUser(userId: number) {
    return this.http.put(this.baseUrl + 'admin/lock/' + userId, {});
}

  getCountForEntities() {
  return this.http.get<Statistics>(this.baseUrl + 'admin/statistics');
}

  showNumberOfDoctors() {
  return this.http.get<any>(this.baseUrl + 'admin/charts1').pipe(
  map( result => {
    console.log(result);
    return result;
  })
  );
}

  showNumberOfOffices() {
  return this.http.get<any>(this.baseUrl + 'admin/charts2').pipe(
  map( result => {
    console.log(result);
    return result;
  })
  );
}

  showNumberOfAppointments() {
  return this.http.get<any>(this.baseUrl + 'admin/charts3').pipe(
  map( result => {
    console.log(result);
    return result;
  })
  );
}
  showNumberOfPatients() {
  return this.http.get<any>(this.baseUrl + 'admin/charts4').pipe(
  map( result => {
    console.log(result);
    return result;
  })
  );
}

}









