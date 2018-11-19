import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '../../../node_modules/@angular/common/http';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getRoles() {
    return this.http.get(this.baseUrl + 'admin/userswithroles');
  }

  updateRoles(user: User, roles: {}) {
    return this.http.post(this.baseUrl + 'admin/editRoles/' + user.userName, roles);
  }

  deleteUser(user: User) {
    return this.http.post(this.baseUrl + 'admin/deleteUser/' + user.userName, {});
  }
}
