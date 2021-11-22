import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { MedicalRecord, MedicalrecordCreateEdit } from '../shared/models/medicalrecord';
import { PaginationForMedicalRecords, PaginationForPatients } from '../shared/models/pagination';
import { Patient, PatientEdit } from '../shared/models/patient';
import { User } from '../shared/models/user';
import { MyParams, UserParams } from '../shared/models/userparams';

@Injectable({
  providedIn: 'root'
})
export class PatientsService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;
  formData: MedicalrecordCreateEdit = new MedicalrecordCreateEdit();
  formData1: PatientEdit = new PatientEdit();

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

  getAllPatientsOfDoctor(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('sort', userParams.sort);
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<PaginationForPatients>(this.baseUrl + 'patients', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  GetMedicalRecordsForAllPatientsOfDoctor(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.officeId !== 0) {
      params = params.append('officeId', userParams.officeId.toString());
    }
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('sort', userParams.sort);
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<PaginationForMedicalRecords>
    (this.baseUrl + 'medicalRecords/allpatientsofdoctor', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getAllMedicalRecordsForPatientAsUser(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('sort', userParams.sort);
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<PaginationForMedicalRecords>
    (this.baseUrl + 'medicalRecords/allrecordsforpatientasuser', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getMedicalRecordsForOnePatientOfDoctor(id: number, myparams: MyParams) {
    let params = new HttpParams();
    if (myparams.query) {
      params = params.append('query', myparams.query);
    }
    params = params.append('sort', myparams.sort);
    params = params.append('page', myparams.page.toString());
    params = params.append('pageCount', myparams.pageCount.toString());
    return this.http.get<PaginationForMedicalRecords>
    (this.baseUrl + 'medicalRecords/onepatientofdoctor/' + id, {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getMedicalRecord(id: number) {
    return this.http.get<MedicalRecord>(this.baseUrl + 'medicalRecords/' + id);
  }

  getPatient(id: number) {
    return this.http.get<Patient>(this.baseUrl + 'patients/' + id);
  }

  getPatientForMyProfile(id: number) {
    return this.http.get<Patient>(this.baseUrl + 'patients/userid/' + id);
  }

  updatingPatientssProfile(formData1){
    return this.http.put(this.baseUrl + 'patients/updatingpatientsprofile/' + formData1.id, formData1);
  }

  createMedicalRecord(values: any) {
    return this.http.post(this.baseUrl + 'medicalRecords/' + this.formData.patientId, values);
  }

}




