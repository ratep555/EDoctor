import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { PaginationFofMedicalRecords } from '../shared/models/pagination';
import { Patient } from '../shared/models/patient';
import { MyParams } from '../shared/models/userparams';

@Injectable({
  providedIn: 'root'
})
export class PatientsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMedicalrecordsForDoctorsPatient(id: number, myparams: MyParams) {
    let params = new HttpParams();
    if (myparams.query) {
      params = params.append('query', myparams.query);
    }
    params = params.append('sort', myparams.sort);
    params = params.append('page', myparams.page.toString());
    params = params.append('pageCount', myparams.pageCount.toString());
    return this.http.get<PaginationFofMedicalRecords>
    (this.baseUrl + 'medicalRecords/doctorspatient/' + id, {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getPatient(id: number) {
    return this.http.get<Patient>(this.baseUrl + 'patients/' + id);
  }

}
