import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MyParams } from '../shared/models/userparams';
import { PaginationForUsers } from '../shared/models/pagination';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

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

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }

  unlockUser(userId: number) {
    return this.http.put(this.baseUrl + 'admin/unlock/' + userId, {});
}

  lockUser(userId: number) {
    return this.http.put(this.baseUrl + 'admin/lock/' + userId, {});
}
}









