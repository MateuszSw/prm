import { element } from 'protractor';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient } from '@angular/common/http';
import {BehaviorSubject} from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwt = new JwtHelperService();
  token: any;
  current: User;
  photoSrc = new BehaviorSubject<string>('../../assets/people.png');
  currentPhoto = this.photoSrc.asObservable();

  constructor(private http: HttpClient) {}

  changeUserPhoto(photoSrc: string) {
    this.photoSrc.next(photoSrc);
  }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.token = this.jwt.decodeToken(user.token);
          this.current = user.user;
          this.changeUserPhoto(this.current.photoSrc);
        }
      })
    );
  }

  register(user: User) {
    return this.http.post(this.baseUrl + 'register', user);
  }

  logIn() {
    const token = localStorage.getItem('token');
    return !this.jwt.isTokenExpired(token);
  }

  role(userRole): boolean {
    let roles = false;
    const userRoles = this.token.role as Array<string>;
    userRole.forEach(e => {
      if (userRoles.includes(e)) {
        roles = true;
        return;
      }
    });
    return roles;
  }
}
